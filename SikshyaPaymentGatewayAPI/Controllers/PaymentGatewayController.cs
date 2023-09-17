using MediatR;
using Microsoft.AspNetCore.Mvc;
using SikshyaPaymentGatewayAPI.Data.Commands;
using SikshyaPaymentGatewayAPI.Data.Queries;
using SikshyaPaymentGatewayAPI.Models;
using SikshyaPaymentGatewayAPI.Services;

namespace SikshyaPaymentGatewayAPI.Controllers
{
    [ApiController]
    [Route("/api/payment-gateway/")]
    public class PaymentGatewayController: Controller
    {
        private IConnectionService _connectionService = null!;
        private readonly IMediator _mediator;
        public PaymentGatewayController(IConnectionService connectionService, IMediator mediator)
        {
            _connectionService = connectionService;
            _mediator = mediator;
        }

        [HttpGet("StudentBalance")]
        public async Task<IActionResult> GetStudentBalance([FromQuery] ShowBalanceModel balanceModel)
        {
            ConnectionStringModel connectionString = new()
            {
                serverIp = balanceModel.serverIp,
                database = balanceModel.database,
                loginId = balanceModel.loginId,
                password = balanceModel.password
            };
            _connectionService.UpdateConnectionString(connectionString);

            var balanceQuery = new GetStudentBalanceQuery(balanceModel.clientId, balanceModel.studentRegistrationNumber);
            var balance = await _mediator.Send(balanceQuery);

            return Ok(balance);
        }

        [HttpPost("ReceiptEntry")]
        public async Task<IActionResult> MakeReceiptEntry(ReceiptEntryModel receiptEntryModel)
        {
            ConnectionStringModel connectionString = new()
            {
                serverIp = receiptEntryModel.serverIp,
                database = receiptEntryModel.database,
                loginId = receiptEntryModel.loginId,
                password = receiptEntryModel.password
            };
            _connectionService.UpdateConnectionString(connectionString);

            var receiptEntryCommand = new ReceiptEntryCommand
            (
                receiptEntryModel.clientId, receiptEntryModel.studentRegistrationNumber,
                receiptEntryModel.paymentAmount, receiptEntryModel.paymentFrom
            );
            var transactionId = await _mediator.Send(receiptEntryCommand);

            return Ok(transactionId);
        }
    }
}
