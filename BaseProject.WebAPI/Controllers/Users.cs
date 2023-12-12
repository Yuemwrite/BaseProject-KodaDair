using BaseProject.Application.Handlers.Users.Commands;
using BaseProject.Application.Handlers.Users.Queries;
using BaseProject.WebAPI.Controllers.BaseController;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BaseProject.WebAPI.Controllers;

public class Users : BaseApiController
{
    /// <summary>Sisteme Kayıt Ol - Tek Kullanımlık Şifre Al</summary>
    /// <response code="201"></response>
    [AllowAnonymous]
    [HttpPost]
    public async Task<IActionResult> Add([FromBody] CreateUserCommand createUserCommand)
    {
        return Created("", await Mediator.Send(createUserCommand));
    }

    /// <summary>Sisteme Kayıt Ol - Tek Kullanımlık Şifre İle Email Doğrula ve Kaydı Tamamla</summary>
    /// <response code="201"></response>
    [AllowAnonymous]
    [HttpPost("approval")]
    public async Task<IActionResult> Add([FromBody] CreateApprovalUserCommand createApprovalUserCommand)
    {
        return Created("", await Mediator.Send(createApprovalUserCommand));
    }
    
    /// <summary>Kullanıcı Takip Et / Takip İsteği Gönder</summary>
    /// <response code="201"></response>
    [HttpPost("follower")]
    public async Task<IActionResult> Add([FromBody] CreateFollowerCommand createFollowerCommand)
    {
        return Created("", await Mediator.Send(createFollowerCommand));
    }
    
    /// <summary>Gelen Takip İsteklerini Listele</summary>
    /// <response code="200"></response>
    [HttpPost("list/pendingfollowersrequest")]
    public async Task<IActionResult> List([FromBody] GetPendingFollowRequestsQuery getPendingFollowRequestsQuery)
    {
        return Created("", await Mediator.Send(getPendingFollowRequestsQuery));
    }
    
    /// <summary>Gelen Takip İsteklerini Onayla/Reddet</summary>
    /// <response code="200"></response>
    [HttpPut("update/pendingfollowersrequest")]
    public async Task<IActionResult> Add([FromBody] UpdateApprovalStatusFollowerCommand updateApprovalStatusFollowerCommand)
    {
        return Created("", await Mediator.Send(updateApprovalStatusFollowerCommand));
    }


    /// <summary>Hesabı Kilitle</summary>
    /// <response code="200"></response>
    [HttpPut("isprivate")]
    public async Task<IActionResult> Add([FromBody] UpdateIsPrivateCommand updateIsPrivateCommand)
    {
        return Created("", await Mediator.Send(updateIsPrivateCommand));
    }
    
    /// <summary>Profili Gör</summary>
    /// <response code="200"></response>
    [HttpGet("profile")]
    public async Task<ActionResult> GetUserProfile()
    {
        return Ok(await Mediator.Send(new GetCurrentUserProfileQuery()));
    }
    
    /// <summary>Profil Bilgilerini Oluştur</summary>
    /// <response code="200"></response>
    [HttpPost("profile/create")]
    public async Task<IActionResult> List([FromBody] CreateUserProfileCommand createUserProfileCommand)
    {
        return Created("", await Mediator.Send(createUserProfileCommand));
    }
    
    /// <summary>Profil Bilgilerini Güncelle</summary>
    /// <response code="200"></response>
    [HttpPut("profile/update")]
    public async Task<IActionResult> Add([FromBody] UpdateUserProfileCommand updateUserProfileCommand)
    {
        return Created("", await Mediator.Send(updateUserProfileCommand));
    }
    
    /// <summary>Eğitim Bilgilerini Oluştur</summary>
    /// <response code="200"></response>
    [HttpPost("education/create")]
    public async Task<IActionResult> CreateEducation([FromBody] CreateEducationCommand createEducationCommand)
    {
        return Created("", await Mediator.Send(createEducationCommand));
    }
    
    /// <summary>Eğitim Bilgilerini Güncelle</summary>
    /// <response code="200"></response>
    [HttpPut("education/update")]
    public async Task<IActionResult> UpdateEducation([FromBody] UpdateEducationCommand updateEducationCommand)
    {
        return Created("", await Mediator.Send(updateEducationCommand));
    }
    
    /// <summary>Eğitim Bilgilerini Sil</summary>
    /// <response code="200"></response>
    [HttpDelete("education/delete")]
    public async Task<IActionResult> DeleteEducation([FromBody] DeleteEducationCommand deleteEducationCommand)
    {
        return Created("", await Mediator.Send(deleteEducationCommand));
    }
    
    /// <summary>Eğitim Bilgilerini Listele</summary>
    /// <response code="200"></response>
    [HttpGet("education/list")]
    public async Task<ActionResult> GetEducation(Guid? userId)
    {
        return Ok(await Mediator.Send(new GetEducationQuery(){UserId = userId}));
    }
    
    /// <summary>İş Deneyim Bilgilerini Listele</summary>
    /// <response code="200"></response>
    [HttpGet("experience/list")]
    public async Task<ActionResult> GetExperiences(Guid? userId)
    {
        return Ok(await Mediator.Send(new GetWorkExperiencesQuery(){UserId = userId}));
    }
    
    /// <summary>İş Deneyim Bilgilerini Oluştur</summary>
    /// <response code="200"></response>
    [HttpPost("experience/create")]
    public async Task<IActionResult> CreateExperience([FromBody] CreateWorkExperienceCommand createWorkExperienceCommand)
    {
        return Created("", await Mediator.Send(createWorkExperienceCommand));
    }
    
    
    /// <summary>İş Deneyim Bilgilerini Güncelle</summary>
    /// <response code="200"></response>
    [HttpPut("experience/update")]
    public async Task<IActionResult> UpdateExperience([FromBody] UpdateWorkExperienceCommand updateWorkExperienceCommand)
    {
        return Created("", await Mediator.Send(updateWorkExperienceCommand));
    }
    
    /// <summary>İş Deneyim Bilgilerini Sil</summary>
    /// <response code="200"></response>
    [HttpDelete("experience/delete")]
    public async Task<IActionResult> DeleteExperience([FromBody] DeleteWorkExperienceCommand deleteWorkExperienceCommand)
    {
        return Created("", await Mediator.Send(deleteWorkExperienceCommand));
    }

    /// <summary>İlgili İş Deneyim Bilgisini Listele</summary>
    /// <response code="200"></response>
    [HttpGet("experience/getbyid")]
    public async Task<ActionResult> GetExperiences(long id)
    {
        return Ok(await Mediator.Send(new GetWorkExperienceQuery(){Id = id}));
    }
    
    [HttpGet("userbenchmark")]
    public async Task<ActionResult> benchmarktest()
    {
        return Ok(await Mediator.Send(new GetBenchMarkQuery()));
    }
    
}
