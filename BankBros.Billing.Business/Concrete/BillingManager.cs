using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Transactions;
using BankBros.Backend.Business.Abstract;
using BankBros.Backend.Business.Constants;
using BankBros.Backend.Core.Aspects.Autofac.Transaction;
using BankBros.Backend.Core.Aspects.Autofac.Validation;
using BankBros.Backend.Core.Entities;
using BankBros.Backend.Core.Utilities.Results;
using BankBros.Backend.DataAccess.Abstract;
using BankBros.Backend.Entity.Concrete;
using BankBros.Backend.Entity.Dtos;
using BankBros.Billing.Business.Abstract;
using BankBros.Billing.Business.Validation.FluentValidation;
using Messages = BankBros.Billing.Business.Constants.Messages;

namespace BankBros.Billing.Business.Concrete
{
    public class BillingManager : IBillingService
    {
        private IBillDal _billDal;
        private ITransactionService _transactionService;

        public BillingManager(IBillDal billDal, ITransactionService transactionService)
        {
            _billDal = billDal;
            _transactionService = transactionService;
        }

        public IDataResult<List<Bill>> GetBills(int customerId)
        {
            try
            {
                return new SuccessDataResult<List<Bill>>(_billDal.GetList(x =>
                    !x.Status &&
                    x.CustomerId.Equals(customerId)
                ).ToList());
            }
            catch (Exception ex)
            {
                return new ErrorDataResult<List<Bill>>(ex.Message);
            }
        }

        [ValidationAspect(typeof(PayBillDtoValidator),Priority = 1)]
        [TransactionScopeAspect(Priority = 2)]
        public IResult PayBill(int billId, int customerId, PayBillDto payBillDto)
        {
            try
            {
                var bill = _billDal.GetSingle(x => x.Id.Equals(billId), y=>y.Organization);
                
                if(bill.CustomerId != customerId)
                    return new ErrorResult(Messages.InvalidCustomerInfo);

                if (bill.Status)
                    return new ErrorResult(Messages.BillAlreadyPaid);

                // Set Amount
                payBillDto.Amount = bill.Amount;
                // Check Charges
                if (bill.LastDateToPay?.Date < DateTime.Now.Date)
                    payBillDto.Amount += bill.Organization.Charge;
                // Make Transaction
                var result = _transactionService.PayBill(customerId,payBillDto);
                if (!result.Success)
                    return new ErrorResult(result.Message);

                // Update Bill Details
                bill.EntityState = EntityState.Modified;
                bill.PaymentAt = DateTime.Now;
                bill.Status = true;
                bill.Organization = null;
                if(_billDal.Update(bill))
                    return new SuccessResult(Messages.BillPaidSuccessfully);

                throw new Exception(Messages.BillPaidFails);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public IResult Add(params Bill[] bills)
        {
            try
            {
                foreach (var bill in bills)
                {
                    bill.CreatedAt = DateTime.Now;
                    bill.PaymentAt = DateTime.Now.AddDays(30);
                    bill.Status = false;
                }
                if(_billDal.Add(bills))
                    return new SuccessResult(Messages.AddingBillsSuccessfully);
                return new ErrorResult(Messages.AddingBillFailed);
            }
            catch (Exception ex)
            {
                return new ErrorResult(ex.Message);
            }
        }
    }
}
