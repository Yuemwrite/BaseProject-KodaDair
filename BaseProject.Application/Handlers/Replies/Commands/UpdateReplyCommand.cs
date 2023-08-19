using AutoMapper;
using BaseProject.Application.Abstraction.Base;
using BaseProject.Application.Common.Exceptions;
using BaseProject.Application.Common.Interfaces;
using BaseProject.Application.Wrapper;
using Domain.Concrete;
using Domain.Dto;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Shared.Common.Extensions;

namespace BaseProject.Application.Handlers.Replies.Commands;

public class UpdateReplyCommand : IRequest<Result>
{
    public Guid Id { get; set; }

    public string Content { get; set; }

    public byte[] RowVersion { get; set; }

    public class UpdateReplyCommandHandler : IRequestHandler<UpdateReplyCommand, Result>
    {
        private readonly ICurrentUser _currentUser;
        private readonly IEntityGeneralRepository _repository;
        private readonly IMapper _mapper;

        public UpdateReplyCommandHandler(ICurrentUser currentUser, IEntityGeneralRepository repository, IMapper mapper)
        {
            _currentUser = currentUser;
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<Result> Handle(UpdateReplyCommand request, CancellationToken cancellationToken)
        {
            var reply = await _repository
                .Query<Reply>()
                .Where(_ => _.Id == request.Id && _.UserId == _currentUser.GetUserId())
                .FirstOrDefaultAsync();

            if (reply is null)
            {
                return (Result)await Result.FailAsync(new ErrorInfo()
                {
                    Code = ((int)ApiException.ExceptionMessages.ReplyNotFound).ToString(),
                    Message = new List<string>() { ApiException.ExceptionMessages.ReplyNotFound.GetCustomDisplayName() }
                });
            }

            reply.Content = reply.Content;
            reply.RowVersion = reply.RowVersion;

            _repository.Update(reply);
            await _repository.SaveChangesAsync();

            var result = _mapper.Map<ReplyDto>(reply);


            return await Result<ReplyDto>.SuccessAsync(result);
        }
    }
}