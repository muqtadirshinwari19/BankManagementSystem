using BankSystem.Data;
using BankSystem.Models.AI;

namespace BankSystem.Services;

public class KnowledgeChunkService
{
    private readonly AppDbContext _context;

    public KnowledgeChunkService(AppDbContext context)
    {
        _context = context;
    }

    public async Task CreateChunksAsync(
        KnowledgeDocument document,
        int chunkSize = 500)
    {
        // Remove old chunks
        var existingChunks = _context.KnowledgeChunks
            .Where(c => c.KnowledgeDocumentId == document.Id);

        _context.KnowledgeChunks.RemoveRange(existingChunks);

        var content = document.Content;

        if (string.IsNullOrWhiteSpace(content))
        {
            return;
        }

        // Normalize PDF text before chunking
        content = content
            .Replace("\r", " ")
            .Replace("\n", " ");

        while (content.Contains("  "))
        {
            content = content.Replace("  ", " ");
        }

        var chunks = new List<KnowledgeChunk>();

        int chunkIndex = 0;
        int position = 0;

        while (position < content.Length)
        {
            int remainingLength = content.Length - position;

            int length = Math.Min(
                chunkSize,
                remainingLength
            );

            // If this is not the last chunk,
            // try to split at a sentence.
            if (position + length < content.Length)
            {
                int lastPeriod = content.LastIndexOf(
                    '.',
                    position + length,
                    length
                );

                if (lastPeriod > position)
                {
                    length = lastPeriod - position + 1;
                }
                else
                {
                    // Otherwise split at a space
                    int lastSpace = content.LastIndexOf(
                        ' ',
                        position + length,
                        length
                    );

                    if (lastSpace > position)
                    {
                        length = lastSpace - position;
                    }
                }
            }

            var chunkContent = content
                .Substring(position, length)
                .Trim();

            if (!string.IsNullOrWhiteSpace(chunkContent))
            {
                chunks.Add(new KnowledgeChunk
                {
                    Content = chunkContent,
                    ChunkIndex = chunkIndex,
                    KnowledgeDocumentId = document.Id
                });

                chunkIndex++;
            }

            position += length;
        }

        await _context.KnowledgeChunks.AddRangeAsync(chunks);

        await _context.SaveChangesAsync();
    }
}