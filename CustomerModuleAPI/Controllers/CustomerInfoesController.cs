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
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using NuGet.Common;


namespace CustomerModuleAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CustomerInfoesController : ControllerBase
    {
        private readonly IConfiguration _iconfiguration;
        private readonly CustomerModuleContext _context;

        public CustomerInfoesController(CustomerModuleContext context)
        {
            _context = context;
        }



        //[HttpGet("GetByEmailID")]
        //public async Task<ActionResult<IEnumerable<CustomerInfo>>> GetByEmailID(string EmailID)
        //{

        //    try
        //    {
        //        var entities = await _context.CustomerInfos.Where(c => c.EmailID == EmailID).ToListAsync();
                
        //        return Ok(entities);
        //    }
        //    catch (Exception ex)
        //    {
        //        // Log the exception or handle it according to your requirements
        //        return StatusCode(500, "Internal server error");
        //    }



        //}

        [HttpGet("ViewListByEmailID") ]
        public async Task<ActionResult<IEnumerable<CustomerInfo>>> GetAllRecord(string EmailID)
        {

            try
            {
                if (EmailID != null)
                {
                    var entities = await _context.CustomerInfos.ToListAsync();
                    return Ok(entities);
                }

                return Ok();

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
                if (!ModelState.IsValid)
                {
                    var errors = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage));
                    return BadRequest(ModelState);

                }

                if (_context.CustomerInfos == null)
                {
                    return Problem("Entity set 'CustomerModuleContext.CustomerInfos'  is null.");
                }

                    _context.CustomerInfos.Add(customer);
                await _context.SaveChangesAsync();
                // return CreatedAtAction(nameof(GetByEmailID), new { EmailID = customer.EmailID }, customer);
                return Ok("Success");
            }
            catch (Exception ex)
            {
                // Log the exception or handle it according to your requirements
                return StatusCode(500, "Internal server error");
            }
        }

        //DELETE: api/CustomerInfoes/5
        [HttpDelete]
        public async Task<IActionResult> DeleteCustomerInfo(Int64 ID)
        {
            try
            {
                if (_context.CustomerInfos == null)
                {
                    return NotFound();
                }
                var customerInfo = await _context.CustomerInfos.FindAsync(ID);
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

        [HttpPut("UpdateCustomerWithID")]
        public async Task<bool> UpdateCustomer(Int64 ID, [FromBody] CustomerInfo C)
        {
            try
            {
                // Retrieve the customer by its ID
                var customer = await _context.CustomerInfos.FirstOrDefaultAsync(c => c.ID == ID);


                if (customer != null)
                {
                    // Modify the customer properties
                    customer.FirstName = C.FirstName;
                    customer.EmailID = C.EmailID;
                    customer.MobileNo = C.MobileNo;


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
