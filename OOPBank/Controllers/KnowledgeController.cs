using BankSystem.Data;
using BankSystem.Models.AI;
using BankSystem.Services;
using Microsoft.AspNetCore.Mvc;

namespace BankSystem.Controllers;

public class KnowledgeController : Controller
{
    private readonly AppDbContext _context;
    private readonly PdfDocumentService _pdfDocumentService;
    private readonly IWebHostEnvironment _environment;
    private readonly KnowledgeChunkService _chunkService;

    public KnowledgeController(
        AppDbContext context,
        PdfDocumentService pdfDocumentService,
        IWebHostEnvironment environment,
        KnowledgeChunkService chunkService)
    {
        _context = context;
        _pdfDocumentService = pdfDocumentService;
        _environment = environment;
        _chunkService = chunkService;
    }

    [HttpGet]
    public IActionResult Upload()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Upload(
        KnowledgeUploadViewModel model)
    {
        if (model.File == null || model.File.Length == 0)
        {
            ModelState.AddModelError(
                "File",
                "Please select a PDF file."
            );
        }

        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var extension = Path.GetExtension(model.File.FileName);

        if (extension.ToLower() != ".pdf")
        {
            ModelState.AddModelError(
                "File",
                "Only PDF files are allowed."
            );

            return View(model);
        }

        var documentsPath = Path.Combine(
            _environment.WebRootPath,
            "documents"
        );

        if (!Directory.Exists(documentsPath))
        {
            Directory.CreateDirectory(documentsPath);
        }

        var fileName =
            $"{Guid.NewGuid()}{extension}";

        var filePath = Path.Combine(
            documentsPath,
            fileName
        );

        using (var stream = new FileStream(
            filePath,
            FileMode.Create))
        {
            await model.File.CopyToAsync(stream);
        }

        var extractedText =
            _pdfDocumentService.ExtractText(filePath);

        if (string.IsNullOrWhiteSpace(extractedText))
        {
            ModelState.AddModelError(
                "File",
                "No readable text was found in this PDF."
            );

            return View(model);
        }

        var knowledgeDocument = new KnowledgeDocument
        {
            Title = model.Title,
            Category = model.Category,
            Content = extractedText
        };

        _context.KnowledgeDocuments.Add(
            knowledgeDocument
        );

        await _context.SaveChangesAsync();

        await _chunkService.CreateChunksAsync(
            knowledgeDocument
        );

        TempData["Success"] =
            "PDF uploaded and processed successfully.";

        return RedirectToAction(nameof(Upload));
    }
}