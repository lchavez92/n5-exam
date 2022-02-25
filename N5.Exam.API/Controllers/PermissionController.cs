using System.Net;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using N5.Exam.Domain.CQRS.Commands;
using N5.Exam.Domain.Models;
using Nest;

namespace N5.Exam.API.Controllers
{

    [Route("api/v1/[controller]s")]
    [ApiController]
    public class PermissionController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IElasticClient _elasticClient;

        public PermissionController(IMediator mediator, IElasticClient elasticClient)
        {
            _mediator = mediator;
            _elasticClient = elasticClient;
        }


        [HttpGet("{permissionId:int}")]
        [ActionName("get")]
        [ProducesResponseType(typeof(PermissionItemDto), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetAsync(int permissionId, CancellationToken cancellationToken = default)
        {
            var permission = await _mediator.Send(new GetPermissionByIdQuery(permissionId), cancellationToken);

            if (permission == null)
            {
                return NotFound();
            }

            return Ok(permission);
        }

        [HttpPost]
        [ActionName("request")]
        [ProducesResponseType(typeof(PermissionItemDto), (int)HttpStatusCode.Created)]
        public async Task<IActionResult> PostAsync(RequestPermissionDto requestPermission,
            CancellationToken cancellationToken = default)
        {
            var permission = await _mediator.Send(new RequestPermissionCommand(requestPermission), cancellationToken);
            await _elasticClient.IndexDocumentAsync(permission, cancellationToken);
            return Created(string.Empty, permission);
        }

        [HttpPut("{permissionId:int}")]
        [ActionName("modify")]
        [ProducesResponseType(typeof(PermissionItemDto), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> PutAsync(int permissionId, ModifyPermissionDto modifyPermission,
            CancellationToken cancellationToken = default)
        {
            var permission = await _mediator.Send(new ModifyPermissionCommand(permissionId, modifyPermission), cancellationToken);

            if (permission == null)
            {
                return NotFound();
            }

            await _elasticClient.IndexDocumentAsync(permission, cancellationToken);
            return Ok(permission);
        }
    }
}
