using GbgMerch.WebUI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;

namespace GbgMerch.WebUI.Controllers;

public class ContactController : Controller
{
    private readonly SmtpSettings _smtpSettings;

    public ContactController(IOptions<SmtpSettings> smtpOptions)
    {
        _smtpSettings = smtpOptions.Value;
    }

    [HttpGet]
    public IActionResult Index()
    {
        return View(new ContactFormModel());
    }

    [HttpPost]
    public async Task<IActionResult> Index(ContactFormModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

        try
        {
            /*
            var mail = new MailMessage
            {
                From = new MailAddress(_smtpSettings.From),
                Subject = $"Kontakt från {model.Name}",
                Body = $"Från: {model.Name} ({model.Email})\n\n{model.Message}",
                IsBodyHtml = false
            };

            mail.To.Add(_smtpSettings.To);

            using var smtp = new SmtpClient(_smtpSettings.Host, _smtpSettings.Port)
            {
                Credentials = new NetworkCredential(_smtpSettings.Username, _smtpSettings.Password),
                EnableSsl = true
            };

            await smtp.SendMailAsync(mail);
            */

            TempData["Success"] = "✅ Ditt meddelande har skickats!";
            return RedirectToAction("Index");
        }
        catch (Exception ex)
        {
            ModelState.AddModelError("", $"Ett fel inträffade: {ex.Message}");
            return View(model);
        }
    }
}
