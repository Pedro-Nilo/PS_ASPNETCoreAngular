using System;
using DutchTreat.Services;
using DutchTreat.ViewModels;
using Microsoft.AspNetCore.Mvc;


namespace DutchTreat.Controllers
{
    public class AppController : Controller
    {
        private readonly IMailService _mailService;


        public AppController(IMailService mailService)
        {
            _mailService = mailService;
        }
        

        public IActionResult Index() 
        {
            return View();
        }

        [HttpGet("contact")]
        public IActionResult Contact()
        {
            return View();
        }

        [HttpPost("contact")]
        public IActionResult Contact(ContactViewModel model)
        {
            if(ModelState.IsValid)
            {
                _mailService.SendMessage("pedroniloz.nicola@gmail.com", model.Subject, $"From: {model.Name} - {model.Email}, Message: {model.Message}");

                ViewBag.UserMessage = "Mail Sent";

                ModelState.Clear();
            }

            return View();
        }

        public IActionResult About()
        {
            return View();
        }
    }
}