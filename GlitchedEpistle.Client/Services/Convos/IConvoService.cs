using System.Threading.Tasks;
using GlitchedPolygons.GlitchedEpistle.Client.Models.DTOs;

namespace GlitchedPolygons.GlitchedEpistle.Client.Services.Convos
{
    /// <summary>
    /// Service interface responsible for accessing convos on the web API (remote).
    /// </summary>
    public interface IConvoService
    {
        Task<string> DownloadAttachment(string attachmentId, string convoId, string userId, string auth);
        Task<string> CreateConvo(ConvoCreationDto convoDto, string userId, string auth);
        Task<bool> DeleteConvo(string convoId, string convoPasswordHash, string userId, string auth);
        Task<bool> PostMessage(string convoId, string convoPasswordHash, string userId, string auth, string senderName, string messageBodiesJson);
        Task<ConvoMetadataDto> GetConvoMetadata(string convoId, string convoPasswordHash, string userId, string auth);
        Task<string> GetConvoMessages(string convoId, string convoPasswordHash, string userId, string auth, int fromIndex = 0);
        Task<int> IndexOf(string convoId, string convoPasswordHash, string userId, string auth, string messageId);
        Task<bool> JoinConvo(string convoId, string convoPasswordHash, string userId, string auth);
        Task<bool> LeaveConvo(string convoId, string convoPasswordHash, string userId, string auth);
        Task<bool> KickUser(string convoId, string convoPasswordHash, string convoAdminId, string auth, string userIdToKick, bool permaBan);
    }
}
