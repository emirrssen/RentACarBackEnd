using Business.Abstract;
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
    public class CarManager : ICarService
    {
        private ICarDal _carDal;

        public CarManager(ICarDal carDal)
        {
            _carDal = carDal;
        }
        public IResult AddCar(Car car)
        {
            _carDal.Add(car);
            return new SuccessResult();
        }

        public IResult DeleteCar(Car car)
        {
            _carDal.Delete(car);
            return new SuccessResult();
        }

        public IDataResult<List<Car>> GetAllCars()
        {
            var result = _carDal.GetAll();
            return new SuccessDataResult<List<Car>>(result);
        }

        public IDataResult<Car> GetCarById(int carId)
        {
            var result = _carDal.Get(x => x.Id == carId);
            return new SuccessDataResult<Car>(result);
        }

        public IResult UpdateCar(Car car)
        {
            _carDal.Update(car);
            return new SuccessResult();
        }
    }
}
