using Insurance.Application.Abstractions.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Insurance.Application.Authentication.Login
{
    public class LoginCommandHandler
    : IRequestHandler<LoginCommand, LoginResponseDto>
    {
        private readonly IUserRepository _userRepository;
        private readonly IJwtTokenGenerator _tokenGenerator;

        public LoginCommandHandler(
            IUserRepository userRepository,
            IJwtTokenGenerator tokenGenerator)
        {
            _userRepository = userRepository;
            _tokenGenerator = tokenGenerator;
        }

        public async Task<LoginResponseDto> Handle(
            LoginCommand request,
            CancellationToken ct)
        {
            var user = await _userRepository
                .GetByUsernameAsync(request.Username, ct);

            if (user is null)
                throw new UnauthorizedAccessException("Invalid credentials");

            var passwordValid = BCrypt.Net.BCrypt.Verify(
                request.Password,
                user.PasswordHash);

            if (!passwordValid)
                throw new UnauthorizedAccessException("Invalid credentials");

            var authUser = new AuthUserContext(
                user.UserId,
                user.Username,
                user.Role,
                user.Role == "Broker" ? user.BrokerId : null);

            var token = _tokenGenerator.Generate(authUser);

            return new LoginResponseDto
            {
                AccessToken = token
            };
        }
    }
}
