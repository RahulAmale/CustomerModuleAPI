using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CustomerModuleAPI.Models;
using Microsoft.Data.SqlClient;
using System.Data;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CustomerModuleAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerInfoesController : ControllerBase
    {
        private readonly IConfiguration _iconfiguration;
        private readonly CustomerModuleContext _context;

        public CustomerInfoesController(CustomerModuleContext context)
        {
            _context = context;
        }


        [HttpGet("GetByID")]
        public async Task<ActionResult<IEnumerable<CustomerInfo>>> GetByID(Int64 ID)
        {

            try
            {
                var entities = await _context.CustomerInfos.Where(c => c.Id == ID).ToListAsync();
                
                return Ok(entities);
            }
            catch (Exception ex)
            {
                // Log the exception or handle it according to your requirements
                return StatusCode(500, "Internal server error");
            }



        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CustomerInfo>>> GetAllRecord( )
        {

            try
            {
                var entities = await _context.CustomerInfos.ToListAsync();

                return Ok(entities);
            }
            catch (Exception ex)
            {
                // Log the exception or handle it according to your requirements
                return StatusCode(500, "Internal server error");
            }



        }

        [HttpPost("AddCustomer")]
        public async Task<ActionResult<CustomerInfo>> AddCustomer(CustomerInfo customer)
        {

            try
            {
                if (_context.CustomerInfos == null)
                {
                    return Problem("Entity set 'CustomerModuleContext.CustomerInfos'  is null.");
                }

                    _context.CustomerInfos.Add(customer);
                await _context.SaveChangesAsync();
                return CreatedAtAction(nameof(GetByID), new { ID = customer.Id }, customer);
            }
            catch (Exception ex)
            {
                // Log the exception or handle it according to your requirements
                return StatusCode(500, "Internal server error");
            }
        }

        //DELETE: api/CustomerInfoes/5
        [HttpDelete]
        public async Task<IActionResult> DeleteCustomerInfo(Int64 id)
        {
            try
            {
                if (_context.CustomerInfos == null)
                {
                    return NotFound();
                }
                var customerInfo = await _context.CustomerInfos.FindAsync(id);
                if (customerInfo == null)
                {
                    return NotFound();
                }


                _context.CustomerInfos.Remove(customerInfo);
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
               // Log the exception or handle it according to your requirements
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPut("ID")]
        public async Task<bool> UpdateCustomer(int ID, string Name,string Address, string MobileNo1, string MobileNo2, string EmailID)
        {
            try
            {
                // Retrieve the customer by its ID
                var customer = await _context.CustomerInfos.FirstOrDefaultAsync(c => c.Id == ID);

                if (customer != null)
                {
                    // Modify the customer properties
                    customer.Id = ID;
                    customer.Name = Name;
                    customer.Address = Address;
                    customer.MobileNo1 = MobileNo1;
                    customer.MobileNo2 = MobileNo2;
                    customer.EmailID = EmailID;

                    // Update the entity in the DbContext
                    _context.CustomerInfos.Update(customer);

                    // Save changes to the database
                    await _context.SaveChangesAsync();
                    return true; // Update successful
                }

                return false; // Customer not found
            }
            catch (Exception ex)
            {
                // Handle exception, log error, or return appropriate response
                return false; // Update failed
            }
        }
    }
}
