using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using _3PsProj.Data;
using _3PsProj.Models;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace _3PsProj.Controllers
{
    [Route("api/ultils/[action]")]
    [ApiController]
    public class CompaniesController : Controller
    {
        public ProjectDbContext _context { get; }

        public CompaniesController(ProjectDbContext context)
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
            var companies = _context.Companies.Include(c => c.Childrens).ToList();
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

        [HttpGet]
        public async Task<ActionResult<Company>> GetCompany(int id)
        {
            var cmp = await _context.Companies.FindAsync(id);
            if (cmp == null)
            {
                return BadRequest();
            }
            return cmp;
        }

        [HttpPost]
        public async Task<ActionResult<Company>> AddCompany(Company company)
        {
            if (company == null)
            {
                return BadRequest();
            }
            var cmp = await _context.Companies.FirstOrDefaultAsync();
            company.Childrens = new List<Company>();
            if (cmp == null)
            {
                _context.Companies.Add(company);
                await _context.SaveChangesAsync();
            }
            else
            {
                cmp.Childrens = new List<Company>();
                cmp.Childrens.Add(company);
                _context.Update(company);
                await _context.SaveChangesAsync();
            }
            return company;
        }

        [HttpPost("[action]/{id}")]
        public async Task<ActionResult<Company>> AddChild(int id, Company company)
        {
            if (company == null)
            {
                return BadRequest();
            }
            var cmp = await _context.Companies.FirstOrDefaultAsync(c => c.Id == id);
            if (cmp == null)
            {
                return BadRequest();
            }
            company.Childrens = new List<Company>();
            cmp.Childrens = new List<Company>();
            cmp.Childrens.Add(company);
            _context.Update(company);
            await _context.SaveChangesAsync();

            return company;
        }

        [HttpPut]
        public async Task<ActionResult<Company>> UpdateCompany(Company company)
        {
            if (company == null)
            {
                return BadRequest();
            }
            company.Childrens = await _context.Companies
                .Where(c => c.CompanyId == company.Id)
                .ToListAsync();
            _context.Update(company);
            await _context.SaveChangesAsync();
            return company;
        }

        [HttpDelete]
        public async Task<ActionResult<Company>> DeleteCompany(int id)
        {
            var company = await _context.Companies.FindAsync(id);
            if (company == null)
            {
                return BadRequest();
            }
            _context.Companies.Remove(company);
            await _context.SaveChangesAsync();
            return company;
        }
    }
}
