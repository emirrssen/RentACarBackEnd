using Business.Abstract;
using Business.ValidationRules;
using Core.AspectMessages;
using Core.Aspects.Autofac.Validation;
using Core.Utilities.AspectResults;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete;
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

        [ValidationAspect(typeof(UserValidator))]
        public IResult AddUser(User user)
        {
            var aspectResult = AspectRules.Check(Results.ValidationResult);

            if (aspectResult != null)
            {
                return new ErrorResult(Results.ValidationResult.Message);
            }
            _userDal.Add(user);
            return new SuccessResult();
        }

        [ValidationAspect(typeof(UserValidator))]
        public IResult DeleteUser(User user)
        {
            var aspectResult = AspectRules.Check(Results.ValidationResult);

            if (aspectResult != null)
            {
                return new ErrorResult(Results.ValidationResult.Message);
            }
            _userDal.Delete(user);
            return new SuccessResult();
        }

        public IDataResult<List<User>> GetAllUsers()
        {
            var result = _userDal.GetAll();
            return new SuccessDataResult<List<User>>(result);
        }

        public IDataResult<User> GetUserById(int userId)
        {
            var result = _userDal.Get(x => x.Id == userId);
            return new SuccessDataResult<User>(result);
        }

        [ValidationAspect(typeof(UserValidator))]
        public IResult UpdateUser(User user)
        {
            var aspectResult = AspectRules.Check(Results.ValidationResult);

            if (aspectResult != null)
            {
                return new ErrorResult(Results.ValidationResult.Message);
            }
            _userDal.Update(user);
            return new SuccessResult();
        }
    }
}
