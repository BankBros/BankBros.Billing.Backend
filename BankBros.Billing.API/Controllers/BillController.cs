using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BankBros.Backend.Entity.Concrete;
using BankBros.Backend.Entity.Dtos;
using BankBros.Billing.Business.Abstract;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BankBros.Billing.API.Controllers
{
    /// <summary>
    /// Faturalandırma servisi.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class BillController : ControllerBase
    {
        private IBillingService _billingService;
        public BillController(IBillingService billingService)
        {
            _billingService = billingService;
        }
        /// <summary>
        /// Müşteriye ait faturaları getirir.
        /// </summary>
        /// <param name="customerId">Müşteri Numarası</param>
        /// <returns></returns>
        [HttpGet("{customerId}")]
        [ProducesResponseType(typeof(List<Bill>), 200)]
        [ProducesResponseType(typeof(string), 400)]
        public IActionResult GetBills(int customerId)
        {
            try
            {
                var result = _billingService.GetBills(customerId);
                if (result.Success)
                    return Ok(result.Data);
                return BadRequest(result.Message);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        /// <summary>
        /// Fatura ödeme işlemini gerçekkleştirir.
        /// </summary>
        /// <param name="customerId">Müşteri Numarası</param>
        /// <param name="billId">Fatura Numarası</param>
        /// <param name="payBillDto">Ödeme Bilgileri</param>
        /// <returns></returns>
        [HttpPost("{customerId}/{billId}")]
        [ProducesResponseType(typeof(string), 200)]
        [ProducesResponseType(typeof(string), 400)]
        public IActionResult Paybill(int customerId,int billId, [FromBody]PayBillDto payBillDto)
        {
            try
            {
                var result = _billingService.PayBill(billId,customerId,payBillDto);
                if (result.Success)
                    return Ok(result.Message);
                return BadRequest(result.Message);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        /// <summary>
        /// Fatura Ekleme işlemini gerçekler.
        /// </summary>
        /// <param name="bill">Fatura</param>
        /// <returns></returns>
        [HttpPost("")]
        [ProducesResponseType(typeof(string), 200)]
        [ProducesResponseType(typeof(string), 400)]
        public IActionResult Paybill([FromBody]Bill bill)
        {
            try
            {
                var result = _billingService.Add(bill);
                if (result.Success)
                    return Ok(result.Message);
                return BadRequest(result.Message);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}