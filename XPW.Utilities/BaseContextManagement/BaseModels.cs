using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using XPW.Utilities.Enums;

namespace XPW.Utilities.BaseContext {
     public class BaseModelGuid {
          public BaseModelGuid() {
               Id = Guid.NewGuid();
               DateCreated = DateTime.Now;
          }
          [Key]
          public Guid Id { get; set; }
          public DateTime DateCreated { get; set; }
          public DateTime? DateUpdated { get; set; }
     }
     public class BaseModelInt {
          public BaseModelInt() {
               DateCreated = DateTime.Now;
          }
          [Key]
          [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
          public int Id { get; set; }
          public DateTime DateCreated { get; set; }
          public DateTime? DateUpdated { get; set; }
     }
     public class BaseModelFile {
          [Key]
          public int Id { get; set; }
          public DateTime DateCreated { get; set; }
     }
     public class BaseModelError {
          public int Id { get; set; }
          public string ErrorCode { get; set; }
          public string Message { get; set; }
          public string Application { get; set; }
          public DateTime DateCreated { get; set; }
          public ErrorResolution Tag { get; set; }
     }
}
