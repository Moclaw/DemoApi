using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using _3PsProj.Data;
using _3PsProj.Models;

namespace _3PsProj.Controllers
{
    [Route("api/ultils/[action]")]
    [ApiController]
    public class CompaniesController : Controller
    {
        public IDbContextFactory<ProjectDbContext> _context;

        public CompaniesController(IDbContextFactory<ProjectDbContext> context)
        {
            _context = context;
        }

        [HttpGet]
        public ActionResult<List<Company>> GetCompanies()
        {
            var companies = GetCompaniesTree();
            if (companies == null)
            {
                return NotFound();
            }

            return companies;
        }

        private List<Company> GetCompaniesTree()
        {
            using (var context = _context.CreateDbContext())
            {
                var companies = context.Companies.Include(c => c.Childrens).ToList();
                var companiesTree = new List<Company>();
                foreach (var company in companies)
                {
                    if (company.CompanyId == null)
                    {
                        companiesTree.Add(company);
                    }
                }
                return companiesTree;
            }
        }

        [HttpGet]
        public async Task<ActionResult<Company>> GetCompany(int id)
        {
            using (var context = _context.CreateDbContext())
            {
                var company = await context.Companies.FindAsync(id);
                if (company == null)
                {
                    return NotFound();
                }
                return company;
            }
        }

        [HttpPost]
        public async Task<ActionResult<Company>> AddCompany(Company company)
        {
            using (var context = _context.CreateDbContext())
            {
                if (company == null)
                {
                    return BadRequest();
                }
                var cmp = await context.Companies.FirstOrDefaultAsync();
                company.Childrens = new List<Company>();
                if (cmp == null)
                {
                    company.CompanyId = null;
                    context.Companies.Add(company);
                    await context.SaveChangesAsync();
                }
                else
                {
                    cmp.Childrens = new List<Company>();
                    cmp.Childrens.Add(company);
                    context.Update(cmp);
                    await context.SaveChangesAsync();
                }
                return company;
            }
        }

        [HttpPost]
        public async Task<ActionResult<Company>> AddChild(int id, Company company)
        {
            using (var context = _context.CreateDbContext())
            {
                if (company == null)
                {
                    return BadRequest();
                }
                var cmp = await context.Companies.FirstOrDefaultAsync(c => c.Id == id);
                if (cmp == null)
                {
                    return BadRequest();
                }
                company.Childrens = new List<Company>();
                cmp.Childrens = new List<Company>();
                cmp.Childrens.Add(company);
                context.Update(company);
                await context.SaveChangesAsync();

                return company;
            }
        }

        [HttpPut]
        public async Task<ActionResult<Company>> EditCompany(Company company)
        {
            using (var context = _context.CreateDbContext())
            {
                if (company == null)
                {
                    return BadRequest();
                }
                var cmp = await context.Companies
                    .Where(c => c.Id == company.Id)
                    .Include(c => c.Childrens)
                    .ThenInclude(c => c.Childrens)
                    .FirstOrDefaultAsync();
                if (cmp == null)
                {
                    return BadRequest();
                }
                cmp.Name = company.Name;
                cmp.Decription = company.Decription;
                cmp.Code = company.Code;
                cmp.Note = company.Note;
                cmp.Radio = company.Radio;
                cmp.Serial = company.Serial;
                context.Update(cmp);
                context.Companies.AsNoTracking();
                await context.SaveChangesAsync();
                return Ok();
            }
        }

        [HttpDelete]
        public async Task<ActionResult<Company>> DeleteCompany(int id)
        {
            using (var context = _context.CreateDbContext())
            {
                var company = await context.Companies
                    .Where(c => c.Id == id)
                    .Include(c => c.Childrens)
                    .ThenInclude(c => c.Childrens)
                    .FirstOrDefaultAsync();
                List<Company> cmp = new List<Company>();
                if (company == null)
                {
                    return BadRequest();
                }
                if (company.Childrens != null)
                {
                    foreach (var item in company.Childrens)
                    {
                        cmp.Add(item);
                    }
                }
                context.Companies.Remove(company);
                context.Companies.RemoveRange(cmp);
                await context.SaveChangesAsync();
                return company;
            }
        }
    }
}
