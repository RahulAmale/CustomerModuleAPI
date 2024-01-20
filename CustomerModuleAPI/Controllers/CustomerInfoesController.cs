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



        [HttpGet]
        public async Task<ActionResult<IEnumerable<CustomerInfo>>> GetAllRecord(string EmailID)
        {

            try
            {

                var CustomerRecordList = await _context.CustomerInfos.ToListAsync();

                CustomerInfo obj = new CustomerInfo();

                // Assuming EmailID is a parameter or variable
                var particularE = CustomerRecordList.FirstOrDefault(c => c.EmailID == EmailID);

                if (particularE != null)
                {
                    // Return the specific record
                    return Ok(particularE);
                }
                else
                {
                    // Return the entire list
                    return Ok(CustomerRecordList);
                }

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

                // Check if the provided EmailID already exists in the database
                bool emailExists = await _context.CustomerInfos.AnyAsync(c => c.EmailID == customer.EmailID);

                if (emailExists)
                {
                    // Return a conflict status indicating that the email already exists
                    return Conflict("Email ID already exists");
                }


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
                return Ok("Record Save Succesfully");
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

        [HttpPut]

        public async Task<IActionResult> UpdateCustomer(Int64 ID, [FromBody] CustomerInfo updatedCustomer)
        {
            try
            {
                // Retrieve the customer by its ID
                var existingCustomer = await _context.CustomerInfos.FindAsync(ID);

                if (existingCustomer == null)
                {
                    // If the customer is not found, return a not found status
                    return NotFound("Customer not found");
                }

                // Check for duplicate EmailID excluding the current record being updated
                bool emailExists = await _context.CustomerInfos
                    .AnyAsync(c => c.EmailID == updatedCustomer.EmailID && c.ID != ID);

                if (emailExists)
                {
                    // Return a conflict status indicating that the email already exists
                    return Conflict("Email ID already exists");
                }

                // Update the customer properties
                existingCustomer.FirstName = updatedCustomer.FirstName;
                existingCustomer.EmailID = updatedCustomer.EmailID;
                existingCustomer.MobileNo = updatedCustomer.MobileNo;

                // Apply any other necessary updates

                // Update the entity in the DbContext
                _context.CustomerInfos.Update(existingCustomer);

                // Save changes to the database
                await _context.SaveChangesAsync();

                return Ok(existingCustomer); // Return the updated customer
            }
            catch (Exception ex)
            {
                // Handle other exceptions, log error, or return appropriate response
                return StatusCode(500, "Internal Server Error");
            }
        }

    }
}
