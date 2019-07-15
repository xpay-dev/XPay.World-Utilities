using System;
using System.ComponentModel.DataAnnotations.Schema;
using XPW.Utilities.Enums;

namespace XPW.Utilities.BaseContext {
     public class BaseModelGuid{
          public BaseModelGuid() {
               Id = Guid.NewGuid();
               DateCreated = DateTime.Now;
          }
          public Guid Id { get; set; }
          public DateTime DateCreated { get; set; }
          public DateTime? DateUpdated { get; set; }

     }
     public class BaseModelInt {
          public BaseModelInt() {
               DateCreated = DateTime.Now;
          }
          [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
          public int Id { get; set; }
          public DateTime DateCreated { get; set; }
          public DateTime? DateUpdated { get; set; }
     }
     public class BaseModelFile {
          public int Id { get; set; }
          public Status Tag { get; set; }
     }
}
