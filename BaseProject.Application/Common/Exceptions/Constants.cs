using System.ComponentModel.DataAnnotations;

namespace BaseProject.Application.Common.Exceptions;

public static class ApiException
{
    public enum ExceptionMessages
    {
        [Display(Name = "Kullanıcı ismi kullanılıyor.", Order = 400)]
        AlreadyExistUserName = 1,
        [Display(Name = "Kategori bilgisi bulunamadı", Order = 400)]
        CategoryNotFound = 2,
        [Display(Name = "Kategori bilgisi bulunamadı", Order = 400)]
        OneTimePasswordNotBeVerified = 3,
        [Display(Name = "Kullanıcı bulunamadı", Order = 400)]
        UserNotFound = 4,
        [Display(Name = "Tek kullanımlık şifreniz geçersiz", Order = 400)]
        InvalidOneTimePassword = 5,
        [Display(Name = "Daha önce profil bilginizi oluşturdunuz", Order = 400)]
        AlreadyExistProfileInfo = 6,
        [Display(Name = "Eğitim bilgisi bulunamadı", Order = 400)]
        EducationNotFound = 7,
        [Display(Name = "Deneyim bilgisi bulunamadı", Order = 400)]
        ExperienceNotFound = 8,
        [Display(Name = "Geçersiz bilgi", Order = 400)]
        InvalidInfo = 9,
        [Display(Name = "Gönderi bulunamadı", Order = 400)]
        SharingNotFound = 10,
        [Display(Name = "Yorum bulunamadı", Order = 400)]
        CommentNotFound = 11,
        [Display(Name = "Yanıt bulunamadı", Order = 400)]
        ReplyNotFound = 12,
        [Display(Name = "Kullanıcı adı veya şifre yanlış", Order = 400)]
        UsernameOrPasswordIsIncorrect= 13,
        [Display(Name = "Profil bilgisi bulunamadı", Order = 400)]
        ProfileInfoNotFound = 14,
        
    }
}