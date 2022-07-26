using CoreLayer.DataAccess.Concrete;
using CoreLayer.DataAccess.Concrete.UnitOfWork;
using DataAccessLayer.Abstract;
using EntityLayer.Models.EntityFremework;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Concreate
{
    public class UnitOfWork : UnitOfWorkBase, IUnitOfWork
    {
        private readonly MyBankContext _context;
        public UnitOfWork(MyBankContext context) : base(context)
        {
            _context = context;
        }

        public IAccountRepository AccountRepository => new AccountRepository(this._context);

        public ICustomerRepository CustomerRepository => new CustomerRepository(this._context);

        public IBranchRepository BranchRepository => new BranchRepository(this._context);

        public IAdressRespository AdressRespository => new AdressRespository(this._context);

        public ICountryRepository CountryRepository => new CountryRepository(this._context);

        public ICityRepository CityRepository => new CityRepository(this._context);

        public IDistrictRepository DistrictRepository => new DistrictRepository(this._context);
        public IAccountBalanceHistoryRepository AccountBalanceHistoryRepository => new AccountBalanceHistoryRepository(this._context);

        public IMailRespository MailRespository => new MailRespository(this._context);

        public IPhoneNumberRespository PhoneNumberRepository => new PhoneNumberRespository(this._context);

        public ICreditCardRespository CreditCardRespository => new CreditCardRespository(this._context);

        public ICardRespository CardRespository => new CardRespository(this._context);

        public IAtmCardRespository AtmCardRespository => new AtmCardRespository(this._context);
    }
}
