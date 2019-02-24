using System.Threading.Tasks;

namespace GlitchedPolygons.GlitchedEpistle.Client.Services.Convos
{
    /// <summary>
    /// Service interface responsible for accessing convos on the web API (remote).
    /// </summary>
    public interface IConvoService
    {
        Task<string> DownloadAttachment(string attachmentId, string convoId, string userId, string auth);
    }
}
