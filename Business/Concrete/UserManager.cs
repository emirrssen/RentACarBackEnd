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
            return new SuccessDataResult<User>(_userDal.Get(user => user.Email == email));

        }

        public IDataResult<List<OperationClaim>> GetClaims(User user)
        {
            return new SuccessDataResult<List<OperationClaim>>(_userDal.GetClaims(user));
        }

        public IResult UpdateProfile(UserForUpdateDto userForUpdate)
        {
            var userToUpdate = GetByMail(userForUpdate.Email).Data;
            var checkedPassword = HashingHelper.VerifyPasswordHash(userForUpdate.Password, userToUpdate.PasswordHash, userToUpdate.PasswordSalt);

            if (!checkedPassword)
            {
                byte[] passwordHash, passwordSalt;
                HashingHelper.CreatePasswordHash(userForUpdate.Password, out passwordHash, out passwordSalt);
                userToUpdate.PasswordHash = passwordHash;
                userToUpdate.PasswordSalt = passwordSalt;

                _userDal.Update(userToUpdate);
                return new SuccessResult(Messages.UserUpdatedSuccessfully);
            }

            return new ErrorResult(Messages.FieldsCannotBeSame);
        }
    }
}
