using System;
using System.Collections.Generic;
using System.Text;
using BankBros.Backend.Core.Utilities.Results;
using BankBros.Backend.Entity.Concrete;
using BankBros.Backend.Entity.Dtos;

namespace BankBros.Billing.Business.Abstract
{
    public interface IBillingService
    {
        IDataResult<List<Bill>> GetBills(int customerId);
        IResult PayBill(int billId, int customerId, PayBillDto payBillDto);
        IResult Add(params Bill[] bills);
    }
}
