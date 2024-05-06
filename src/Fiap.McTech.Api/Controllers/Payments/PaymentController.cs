﻿using Fiap.McTech.Application.ViewModels.Payments;
using Microsoft.AspNetCore.Mvc;
using Fiap.McTech.Application.Interfaces;

namespace Fiap.McTech.Api.Controllers.Payments
{
    public class PaymentController : Controller
	{
		public readonly IPaymentAppService _PaymentAppService;

		public PaymentController(IPaymentAppService PaymentAppService)
		{
			_PaymentAppService = PaymentAppService;
		}

		[HttpGet("Payment")]
		public async Task<PaymentOutputViewModel> GetPayments()
		{
			return new PaymentOutputViewModel();
		}
	}
}