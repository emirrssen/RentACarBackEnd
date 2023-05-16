using Business.Abstract;
using Business.Constants;
using Business.ValidationRules;
using Core.Aspects.Autofac.Validation;
using Core.Entities.Concrete;
using Core.Utilities.Results;
using Core.Utilities.Security.Hashing;
using DataAccess.Abstract;
using Entities.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Concrete
{
    public class UserManager : IUserService
    {
        
        IUserDal _userDal;

        public UserManager(IUserDal userDal)
        {
            _userDal = userDal;
        }

        public IResult Add(User user)
        {
            _userDal.Add(user);
            return new SuccessResult();
        }

        public IDataResult<User> GetByMail(string email)
        {
            var result = _userDal.Get(user => user.Email == email);

            if (result == null)
            {
                return new ErrorDataResult<User>(Messages.UserNotFound);
            }

            return new SuccessDataResult<User>(result);

        }

        public IDataResult<List<OperationClaim>> GetClaims(User user)
        {
            return new SuccessDataResult<List<OperationClaim>>(_userDal.GetClaims(user));
        }

        public IResult UpdateProfile(UserForUpdateDto userForUpdate)
        {
            var userToUpdate = GetByMail(userForUpdate.Email);

            if (userToUpdate.Success)
            {
                userToUpdate.Data.FirstName = userForUpdate.FirstName;
                userToUpdate.Data.LastName = userForUpdate.LastName;
                _userDal.Update(userToUpdate.Data);

                return new SuccessResult(Messages.UserUpdatedSuccessfully);
            }

            return new ErrorResult(userToUpdate.Message);
        }

        public IDataResult<List<User>> GetAllUsers()
        {
            var result = _userDal.GetAll();
            return new SuccessDataResult<List<User>>(result, Messages.UsersListed);
        }

        public IResult UpdatePassword(string oldPassword, string newPassword, UserForUpdateDto userForUpdate)
        {
            var userToUpdate = GetByMail(userForUpdate.Email);
            bool checkedPassword = false;

            if (userToUpdate.Success)
            {
                checkedPassword = HashingHelper.VerifyPasswordHash(oldPassword, userToUpdate.Data.PasswordHash, userToUpdate.Data.PasswordSalt);
            } else
            {
                return new ErrorResult(Messages.UserNotFound);
            }

            if (checkedPassword)
            {
                byte[] passwordHash, passwordSalt;
                HashingHelper.CreatePasswordHash(newPassword, out passwordHash, out passwordSalt);
                userToUpdate.Data.PasswordHash = passwordHash;
                userToUpdate.Data.PasswordSalt = passwordSalt;

                _userDal.Update(userToUpdate.Data);
                return new SuccessResult(Messages.UserUpdatedSuccessfully);
            }

            return new ErrorResult(Messages.OldPasswordIncorrect);
        }
    }
}
