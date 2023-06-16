using Business.Abstract;
using Business.Constants;
using Business.ValidationRules;
using Core.Aspects.Autofac.Validation;
using Core.Entities.Concrete;
using Core.Utilities.Results;
using Core.Utilities.Security.Hashing;
using Core.Utilities.Security.JWT;
using Entities.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Concrete
{
    public class AuthManager : IAuthService
    {
        private IUserService _userService;
        private ITokenHelper _tokenHelper;
        private IUserOperationClaimService _userOperationClaimService;

        public AuthManager(IUserService userService, ITokenHelper tokenHelper, IUserOperationClaimService userOperationClaimService)
        {
            _userService = userService;
            _tokenHelper = tokenHelper;
            _userOperationClaimService = userOperationClaimService;

        }
        public IDataResult<AccessToken> CreateAccessToken(User user, List<OperationClaim> claims)
        {
            var accessToken = _tokenHelper.CreateToken(user, claims);
            return new SuccessDataResult<AccessToken>(accessToken, Messages.AccessTokenCreated);
        }

        public IDataResult<LoginResultDto> Login(UserForLoginDto userForLoginDto)
        {
            var userResult = LoginTool(userForLoginDto);
            var claims = _userService.GetClaims(userResult.Data).Data;

            if (userResult.Success)
            {
                var accessToken = CreateAccessToken(userResult.Data, claims);
                var loginResult = new LoginResultDto
                {
                    AccessToken = accessToken.Data,
                    FirstName = userResult.Data.FirstName,
                    LastName = userResult.Data.LastName,
                    Email = userResult.Data.Email,
                    Claims = string.Join(",", claims.Select(claim => claim.Name))
                };

                return new SuccessDataResult<LoginResultDto>(loginResult, Messages.AccessTokenCreated);
            }

            return new ErrorDataResult<LoginResultDto>(userResult.Message);
        }

        [ValidationAspect(typeof(UserForRegisterValidator))]
        public IDataResult<UserForRegisterDto> Register(UserForRegisterDto userForRegisterDto)
        {
            var result = RegisterTool(userForRegisterDto);
            SetDefaultClaim(userForRegisterDto.Email);
            return result;
        }

        public IResult UserExists(string email)
        {
            if(_userService.GetByMail(email) != null)
            {
                return new ErrorResult(Messages.UserAlreadyExists);
            }
            return new SuccessResult();
        }

        private IDataResult<UserForRegisterDto> RegisterTool(UserForRegisterDto userForRegisterDto)
        {
            byte[] passwordHash, passwordSalt;
            HashingHelper.CreatePasswordHash(userForRegisterDto.Password, out passwordHash, out passwordSalt);
            var user = new User
            {
                Email = userForRegisterDto.Email,
                FirstName = userForRegisterDto.FirstName,
                LastName = userForRegisterDto.LastName,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt,
                Status = true
            };

            _userService.Add(user);
            return new SuccessDataResult<UserForRegisterDto>(userForRegisterDto, Messages.UserRegisteredSuccessfully);
        }

        private IDataResult<User> LoginTool(UserForLoginDto userForLoginDto)
        {
            var userToCheck = _userService.GetByMail(userForLoginDto.Email).Data;
            if (userToCheck == null)
            {
                return new ErrorDataResult<User>(Messages.UserNotFound);
            }

            if (!HashingHelper.VerifyPasswordHash(userForLoginDto.Password, userToCheck.PasswordHash, userToCheck.PasswordSalt))
            {
                return new ErrorDataResult<User>("Wrong password!");
            }

            return new SuccessDataResult<User>(userToCheck, Messages.SuccessfulLogin);
        }

        public void SetDefaultClaim(string email)
        {
            var user = _userService.GetByMail(email).Data;
            _userOperationClaimService.AddDefaulClaim(user);
        }


    }
}
