using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace NeedsSoySauce.Models
{
    public enum NotificationType
    {
        GoonsquadMembership
    }

    public abstract class NotificationBase
    {
        public NotificationType NotificationType { get; set; }
    }

    public class GoonsquadMembershipNotification : NotificationBase
    {
        public Guid GoonsquadId { get; set; }

        public GoonsquadMembershipNotification()
        {
            NotificationType = NotificationType.GoonsquadMembership;
        }
    }
}