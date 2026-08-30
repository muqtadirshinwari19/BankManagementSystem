using BankSystem.Models.AI;
using BankSystem.Services;

namespace BankSystem.Data;

public static class KnowledgeSeeder
{
    public static async Task SeedAsync(
        AppDbContext context,
        KnowledgeChunkService chunkService)
    {
        if (!context.KnowledgeDocuments.Any())
        {
            var documents = new List<KnowledgeDocument>
            {
               

                
            };

            await context.KnowledgeDocuments.AddRangeAsync(documents);

            await context.SaveChangesAsync();
        }

        var allDocuments = context.KnowledgeDocuments.ToList();

        foreach (var document in allDocuments)
        {
            var hasChunks = context.KnowledgeChunks
                .Any(c => c.KnowledgeDocumentId == document.Id);

            if (!hasChunks)
            {
                await chunkService.CreateChunksAsync(document);
            }
        }
    }
}