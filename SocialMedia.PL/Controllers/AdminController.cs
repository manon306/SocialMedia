public class AdminController : Controller
{
    private readonly UserManager<User> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;

    public AdminController(UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
    {
        _userManager = userManager;
        _roleManager = roleManager;
    }

    // 🟢 صفحة تعرض كل اليوزرز
    public IActionResult ManageRoles()
    {
        var users = _userManager.Users.ToList();
        return View(users);
    }

    // 🟢 Action لإضافة Role ليوزر
    [HttpPost]
    [HttpPost]
    public async Task<IActionResult> AssignRole(string userId, string roleName)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
        {
            return NotFound("User not found");
        }

        if (!await _roleManager.RoleExistsAsync(roleName))
        {
            await _roleManager.CreateAsync(new IdentityRole(roleName));
        }

        var result = await _userManager.AddToRoleAsync(user, roleName);

        if (result.Succeeded)
        {
            return Ok("Role assigned successfully");
        }

        var errors = string.Join(", ", result.Errors.Select(e => e.Description));
        return BadRequest($"Failed to assign role: {errors}");
    }

}
