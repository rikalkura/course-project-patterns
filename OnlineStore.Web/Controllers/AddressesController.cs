using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineStore.Core.Entities;
using OnlineStore.Core.Interfaces;
using System.Security.Claims;

namespace OnlineStore.Web.Controllers;

[Authorize]
public class AddressesController : Controller
{
    private readonly IAddressRepository _addressRepository;

    public AddressesController(IAddressRepository addressRepository)
    {
        _addressRepository = addressRepository;
    }

    // GET: Addresses/Create
    public IActionResult Create(string? returnUrl = null)
    {
        ViewBag.ReturnUrl = returnUrl ?? Url.Action("Index", "Orders");
        return View(new Address());
    }

    // POST: Addresses/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Street,City,State,ZipCode,Country,IsDefault")] Address address, string? returnUrl = null)
    {
        var clientId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(clientId))
        {
            TempData["Error"] = "User not authenticated.";
            ViewBag.ReturnUrl = returnUrl ?? Url.Action("Index", "Orders");
            return View(address);
        }

        // Manually validate required fields
        if (string.IsNullOrWhiteSpace(address.Street))
        {
            ModelState.AddModelError(nameof(address.Street), "Street address is required.");
        }
        if (string.IsNullOrWhiteSpace(address.City))
        {
            ModelState.AddModelError(nameof(address.City), "City is required.");
        }
        if (string.IsNullOrWhiteSpace(address.Country))
        {
            ModelState.AddModelError(nameof(address.Country), "Country is required.");
        }

        // Remove ModelState error for Client navigation property since we set ClientId directly
        ModelState.Remove(nameof(address.Client));
        ModelState.Remove(nameof(address.ClientId));

        if (!ModelState.IsValid)
        {
            ViewBag.ReturnUrl = returnUrl ?? Url.Action("Index", "Orders");
            return View(address);
        }

        try
        {
            address.ClientId = clientId;
            
            // If this is the first address, make it default
            var existingAddresses = await _addressRepository.GetByClientIdAsync(clientId);
            if (!existingAddresses.Any())
            {
                address.IsDefault = true;
            }
            else if (address.IsDefault)
            {
                // If setting as default, unset other default addresses
                foreach (var existingAddress in existingAddresses.Where(a => a.IsDefault))
                {
                    existingAddress.IsDefault = false;
                    await _addressRepository.UpdateAsync(existingAddress);
                }
            }

            // Add address using repository
            await _addressRepository.AddAsync(address);
            await _addressRepository.SaveChangesAsync();

            if (!string.IsNullOrEmpty(returnUrl))
            {
                TempData["Success"] = "Address added successfully!";
                return Redirect(returnUrl);
            }

            TempData["Success"] = "Address added successfully!";
            return RedirectToAction("Index", "Orders");
        }
        catch (Exception ex)
        {
            TempData["Error"] = $"An error occurred while saving the address: {ex.Message}";
            ViewBag.ReturnUrl = returnUrl ?? Url.Action("Index", "Orders");
            return View(address);
        }
    }
}

