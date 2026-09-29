using GymManagmentSystem.BLL.Services.Attachment;
using GymManagmentSystem.BLL.Services.Classes;
using GymManagmentSystem.BLL.Services.Interfaces;
using GymManagmentSystem.BLL.ViewModels.MemberViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace GymManagmentSystem.APi.Controllers
{
    [Authorize(Roles = "SuperAdmin")]

    public class MemberController : Controller
    {
        private readonly IMemberServices _memberservices;
        private readonly IAttachmentServices _attachmentServices;

        public MemberController(IMemberServices memberServices , IAttachmentServices attachment)
        {
            _memberservices = memberServices;
            _attachmentServices = attachment;
        }
        public async Task<IActionResult> Index( CancellationToken ct = default)
        {
            var result = await _memberservices.GetMembersAsync(ct);

            if (!result.success)
            {
                return View(Enumerable.Empty<GetMemberViewModels>());
            }

            return View(result.value);
        }


        [HttpGet]
        public async Task<IActionResult> Picture(
    int id,
    CancellationToken ct = default)
        {
            var member = await _memberservices.GetMemberDetailsById(id, ct);

            if (!member.success ||
                member.value is null ||
                string.IsNullOrWhiteSpace(member.value.Photo))
            {
                return NotFound();
            }

            var result = _attachmentServices.GetFile(
                member.value.Photo,
                "MembersPhoto");

            if (!result.success)
                return NotFound();

            return File(
                result.value.stream,
                result.value.contentType);
        }

        [HttpGet]
         
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]

        public async Task<IActionResult> CreateMember(CreateMemberViewModel model , CancellationToken ct)
        {
            if (!ModelState.IsValid) return View(nameof(Create) , model);

            var result = await _memberservices.CreateMemberAsync(model, ct);

            if (result.success)
                TempData["SuccessMessage"] = "Member Created Successfully";
            else
                TempData["ErrorMessage"] = result.error;

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> MemberDetails(int id, CancellationToken cancellationToken)
        {
            var memberDetails = await _memberservices.GetMemberDetailsById(id, cancellationToken);
            if (!memberDetails.success)
            {
                TempData["ErrorMessage"] = "Member Not Found";
                return RedirectToAction(nameof(Index));
            }
            return View(memberDetails.value);
        }

        public async Task<IActionResult> HealthRecordDetails(int id, CancellationToken cancellationToken)
        {
            var record = await _memberservices.GetHealthRecoerd(id, cancellationToken);
            if(!record.success)
            {
                TempData["Error Message"] = "Health Record Not Found";
                return RedirectToAction(nameof(Index));
            }
            return View(record.value);
        }

        [HttpGet]
        public async Task<IActionResult> EditMember (int id , CancellationToken cancellationToken)
        {
            var member = await _memberservices.GetToMemberToUpdate(id, cancellationToken);
            if(!member.success)
            {
                TempData["ErrorMessage"] = "Member Not Found";
                return RedirectToAction(nameof(Index));
            }
            return View(member.value);
        }

        [HttpPost]

        public async Task<IActionResult> EditMember(int id ,UpdateToMemberViewModel model , CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid) return View(model);

            var result = await _memberservices.UpdateToMemberDetails(id, model, cancellationToken);
            if (result.success)
                TempData["SuccessMessage"] = "Member Updated Successfully";
            else
                TempData["ErrorMessage"] = "Member Failed To Updated";

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Delete (int id , CancellationToken ct)
        {
            var result = await _memberservices.GetMemberDetailsById(id, ct);
            if (result is null)
            {
                TempData["ErrorMessage"] = "Faild To Delete Member";
                return RedirectToAction(nameof (Index));
            }

            return View();
        }

        [HttpPost]

        public async Task<IActionResult> DeleteConfirmed(int id , CancellationToken ct)
        {
            var member = await _memberservices.DeleteMemberAsync(id, ct);
            if (member.success)
                TempData["SuccessMessage"] = "Member Deleted Successfully";
            else
                TempData["ErrorMessage"] = "Member Failed To Delete";
            

            return RedirectToAction(nameof(Index));
        }
    }
}
