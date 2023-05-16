using Business.Abstract;
using Business.Constants;
using Business.ValidationRules;
using Core.Aspects.Autofac.Validation;
using Core.Utilities.Business;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete;
using Entities.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Concrete
{
    public class RentalManager : IRentalService
    {
        IRentalDal _rentalDal;
        IUserService _userService;

        public RentalManager(IRentalDal rentalDal, IUserService userService)
        {
            _rentalDal = rentalDal;
            _userService = userService;
        }

        [ValidationAspect(typeof(RentalValidator))]
        public IResult AddRental(Rental rental)
        {
            var businessResult = BusinessRules.Run(ChecKIfCarAvailable(rental.CarId));

            if (businessResult != null)
            {
                return new ErrorResult(businessResult.Message);
            }
            _rentalDal.Add(rental);
            return new SuccessResult(Messages.RentalAddedSuccessfully);
        }

        [ValidationAspect(typeof(RentalValidator))]
        public IResult DeleteRental(Rental rental)
        {
            _rentalDal.Delete(rental);
            return new SuccessResult(Messages.RentalDeletedSuccessfully);
        }

        public IDataResult<List<Rental>> GetAllRentals()
        {
            var result = _rentalDal.GetAll();
            return new SuccessDataResult<List<Rental>>(result, Messages.RentalsListedSuccessfully);
        }

        [ValidationAspect(typeof(RentalValidator))]
        public IResult UpdateRental(Rental rental)
        {
            _rentalDal.Update(rental);
            return new SuccessResult(Messages.RentalUpdatedSuccessfully);
        }

        private IResult ChecKIfCarAvailable(int carId)
        {
            var result = _rentalDal.GetAll(x => x.CarId == carId);
            foreach (var record in result)
            {
                if (record.ReturnDate == null)
                {
                    return new ErrorResult(Messages.CarNotAvailable);
                }
            }
            return new SuccessResult();
        }

        public IDataResult<List<RentalForListDto>> GetRentalsByUserId(string email)
        {
            var userResult = _userService.GetByMail(email);

            if (userResult.Success)
            {
                var result = _rentalDal.GetDetailedRentals(userResult.Data.Id);
                return new SuccessDataResult<List<RentalForListDto>>(result, Messages.RentalsListedSuccessfully);
            }

            return new ErrorDataResult<List<RentalForListDto>>(Messages.UserNotFound);

        }
    }
}
