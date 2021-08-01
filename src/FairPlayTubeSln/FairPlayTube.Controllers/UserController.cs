﻿using FairPlayTube.Common.Global.Enums;
using FairPlayTube.Common.Interfaces;
using FairPlayTube.DataAccess.Data;
using FairPlayTube.Models.Invites;
using FairPlayTube.Models.UserMessage;
using FairPlayTube.Models.UserProfile;
using FairPlayTube.Notifications.Hubs;
using FairPlayTube.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.FeatureManagement.Mvc;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FairPlayTube.Controllers
{
    /// <summary>
    /// Handles all of the data regarding a User
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserController : ControllerBase
    {
        private FairplaytubeDatabaseContext FairplaytubeDatabaseContext { get; }
        private ICurrentUserProvider CurrentUserProvider { get; }
        private EmailService EmailService { get; }
        private MessageService MessageService { get; }
        /// <summary>
        /// Initializes <see cref="UserController"/>
        /// </summary>
        /// <param name="fairplaytubeDatabaseContext"></param>
        /// <param name="currentUserProvider"></param>
        /// <param name="emailService"></param>
        /// <param name="messageService"></param>
        public UserController(FairplaytubeDatabaseContext fairplaytubeDatabaseContext,
            ICurrentUserProvider currentUserProvider, EmailService emailService,
            MessageService messageService)
        {
            this.FairplaytubeDatabaseContext = fairplaytubeDatabaseContext;
            this.CurrentUserProvider = currentUserProvider;
            this.EmailService = emailService;
            this.MessageService = messageService;
        }

        /// <summary>
        /// Gets the name of the role assigned to the Logged In User
        /// </summary>
        /// <returns></returns>
        [HttpGet("[action]")]
        [Authorize]
        public async Task<string> GetMyRole(CancellationToken cancellationToken)
        {
            var userAdB2CObjectId = this.CurrentUserProvider.GetObjectId();
            var role = await this.FairplaytubeDatabaseContext.ApplicationUserRole
                .Include(p => p.ApplicationUser)
                .Include(p => p.ApplicationRole)
                .Where(p => p.ApplicationUser.AzureAdB2cobjectId.ToString() == userAdB2CObjectId)
                .Select(p => p.ApplicationRole.Name).SingleOrDefaultAsync(cancellationToken: cancellationToken);
            return role;
        }

        /// <summary>
        /// List the users in the system
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet("[action]")]
        public async Task<UserModel[]> ListUsers(CancellationToken cancellationToken)
        {
            var result = await this.FairplaytubeDatabaseContext.ApplicationUser
                .Include(p => p.VideoInfo)
                .Include(p => p.Brand)
                .Select(p => new UserModel
                {
                    ApplicationUserId = p.ApplicationUserId,
                    Name = p.FullName,
                    BrandsCount = p.Brand.Count,
                    VideosCount = p.VideoInfo.Count
                }).ToArrayAsync();
            return result;
        }

        /// <summary>
        /// Invites a user to use the system
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("[action]")]
        public async Task InviteUser(InviteUserModel model)
        {
            var userName = this.CurrentUserProvider.GetUsername();
            StringBuilder completeBody = new StringBuilder(model.CustomMessage);
            completeBody.AppendLine();
            string link = $"<a href='{this.Request.Host.Value}'>{this.Request.Host.Value}</a>";
            completeBody.AppendLine(link);
            await this.EmailService.SendEmail(model.ToEmailAddress, $"{userName} is inviting you to " +
                $"FairPlayTube: The Next Generation Of Video Sharing Portals",
                completeBody.ToString(), true);
        }

        /// <summary>
        /// Sends a message to the specified user
        /// </summary>
        /// <param name="model"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost("[action]")]
        [FeatureGate(FeatureType.PaidFeature)]
        public async Task SendMessage(UserMessageModel model, CancellationToken cancellationToken)
        {
            var senderObjectId = this.CurrentUserProvider.GetObjectId();
            await this.MessageService.SendMessageAsync(model, senderObjectId, cancellationToken);
        }
    }
}