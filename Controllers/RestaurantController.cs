using Microsoft.AspNetCore.Mvc;
using RestaurantManage.Repositories;
using RestaurantManage.Models;
using NuGet.Protocol.Core.Types;

namespace RestaurantManage.Controllers
{
    public class RestaurantController : Controller
    {
        private readonly RegistrationRepository _registrationRepository;
        private readonly LoginRepository _loginRepository;
        private readonly ContactRepository _contactRepository;
        private readonly CategoryRepository _categoryRepository;
        private readonly ReservationRepository _reservationRepository;
        private readonly DishRepository _dishRepository;
        private readonly AddTableRepository _addTableRepository;

        public RestaurantController(RegistrationRepository registrationRepository, 
            LoginRepository loginRepository,
            ContactRepository contactRepository, 
            CategoryRepository categoryRepository,
            ReservationRepository reservationRepository,
            DishRepository dishRepository,
            AddTableRepository addTableRepository)
        {
            _registrationRepository = registrationRepository;
            _loginRepository = loginRepository;
            _contactRepository = contactRepository;
            _categoryRepository = categoryRepository;
            _reservationRepository = reservationRepository;
            _dishRepository = dishRepository;
            _addTableRepository = addTableRepository;
        }
        //Main Home Page
        public IActionResult Home()
        {
            return View();
        }

        public IActionResult AboutUs()
        {
            return View();
        }
        //Contact 
        public IActionResult Contact()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Contact(Contact contact)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _contactRepository.AddContact(contact);
                    return RedirectToAction("Contact", "Restaurant");
                }
            }
            catch (Exception ex)
            {

                ViewData["ErrorMessage"] = ex.Message;
                return View("MyError");
            }

            return View(contact);
        }
        [HttpGet]
        public IActionResult ContactMessages()
        {
            try
            {
                var contacts = _contactRepository.GetAllContacts();
                return View(contacts);
            }
            catch (Exception ex)
            {

                ViewData["ErrorMessage"] = ex.Message;
                return View("MyError");
            }
        }
        [HttpPost]
        public IActionResult DeleteContactMessage(int id)
        {
            try
            {
                _contactRepository.DeleteContact(id);
                return RedirectToAction("ContactMessages");
            }
            catch (Exception ex)
            {
                ViewData["ErrorMessage"] = ex.Message;
                return View("MyError");
            }
        }
        //get :All Users
        public IActionResult AllUsers()
        {
            try
            {
                var users = _registrationRepository.GetAllUsers();
                return View(users);
            }
            catch (Exception ex)
            {
                ViewData["ErrorMessage"] = ex.Message;
                return View("MyError");
            }
        }
        //View for seeing deleting users datails
        public IActionResult Delete(int id)
        {
            try
            {
                var user = _registrationRepository.GetUserByID(id);
                if (user == null)
                {
                    return NotFound();
                }
                return View(user);
            }
            catch (Exception ex)
            {
                ViewData["ErrorMessage"] = ex.Message;
                return View("MyError");
            }
        }
        // GET: User Registration Page
        public IActionResult RegisterUser()
        {
            try
            {
                var registration = new Registration
                {
                    UserType = "user"
                };
                return View(registration);
            }
            catch (Exception ex)
            {
                ViewData["ErrorMessage"] = ex.Message;
                return View("MyError");
            }
        }
        // POST: Register User
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult RegisterUser(Registration registration)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _registrationRepository.AddUser(registration);
                    return RedirectToAction("Login", "Restaurant");
                }
                return View(registration);
            }
            catch (Exception ex)
            {
                ViewData["ErrorMessage"] = ex.Message;
                return View("MyError");
            }
        }
        // GET: Admin Registration Page
        public IActionResult RegisterAdmin()
        {
            try
            {
                var registration = new Registration
                {
                    UserType = "admin"
                };
                return View(registration);
            }
            catch (Exception ex)
            {
                ViewData["ErrorMessage"] = ex.Message;
                return View("MyError");
            }
        }
        // POST: Register Admin
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult RegisterAdmin(Registration registration)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _registrationRepository.AddUser(registration);
                    return RedirectToAction("AllUsers", "Restaurant");
                }
                return View(registration);
            }
            catch (Exception ex)
            {
                ViewData["ErrorMessage"] = ex.Message;
                return View("MyError");
            }
        }
        // GET: All Registered Users
        public IActionResult GetRegisteredUsers()
        {
            try
            {
                List<Registration> users = _registrationRepository.GetRegisteredUsers();
                return View(users);
            }
            catch (Exception ex)
            {
                ViewData["ErrorMessage"] = ex.Message;
                return View("MyError");
            }
        }
        // GET: All Registered Admins
        public IActionResult GetRegisteredAdmins()
        {
            try
            {
                List<Registration> admins = _registrationRepository.GetRegisteredAdmins();
                return View(admins);
            }
            catch (Exception ex)
            {
                ViewData["ErrorMessage"] = ex.Message;
                return View("MyError");
            }
        }
        //Login View
        [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
        [HttpGet]
        public IActionResult Login()
        {
            return View(new Login());
        }
        // POST: Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(Login login)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(login);
                }

                var user = _loginRepository.ValidateUser(login.Username, login.Password);

                if (user == null)
                {
                    ModelState.AddModelError(string.Empty, "Invalid username or password.");
                    return View(login);
                }
                // Store the User ID in session
                HttpContext.Session.SetInt32("UserID", user.ID);
                if (user.UserType == "admin")
                {
                    return RedirectToAction("AdminHome");
                }
                else
                {
                    return RedirectToAction("UserHome");
                }
            }
            catch (Exception ex)
            {
                ViewData["ErrorMessage"] = ex.Message;
                return View("MyError");
            }
        }
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();  
            return RedirectToAction("Login", "Restaurant");
        }
        // Admin Home
        public IActionResult AdminHome(string name)
        {
            try
            {
                int? userId = HttpContext.Session.GetInt32("UserID");
                if (userId == null)
                {
                    return RedirectToAction("Login", "Restaurant");
                }

                var admin = _registrationRepository.GetUserByID(userId.Value);

                if (admin == null || admin.UserType != "admin")
                {
                    return RedirectToAction("Login", "Restaurant");
                }

                return View(admin);
            }
            catch (Exception ex)
            {
                ViewData["ErrorMessage"] = ex.Message;
                return View("MyError");
            }
        }
        // User Home
        public IActionResult UserHome(string name)
        {
            try
            {
                int? userId = HttpContext.Session.GetInt32("UserID");
                if (userId == null)
                {
                    return RedirectToAction("Login", "Restaurant");
                }

                var user = _registrationRepository.GetUserByID(userId.Value);

                if (user == null || user.UserType != "user")
                {
                    return RedirectToAction("Login", "Restaurant");
                }

                return View(user);
            }
            catch (Exception ex)
            {
                ViewData["ErrorMessage"] = ex.Message;
                return View("MyError");
            }
        }
        // Edit User
        public IActionResult EditUser()
        {
            try
            {
                int? userId = HttpContext.Session.GetInt32("UserID");
                if (userId == null)
                {
                    return RedirectToAction("Login", "Restaurant");
                }

                var user = _registrationRepository.GetUserByID(userId.Value);
                if (user == null)
                {
                    return NotFound();
                }

                return View(user);
            }
            catch (Exception ex)
            {
                ViewData["ErrorMessage"] = ex.Message;
                return View("MyError");
            }
        }
        // POST: Edit User
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditUser(Registration registration)
        {
            try
            {
                int? userId = HttpContext.Session.GetInt32("UserID");
                if (userId == null || userId != registration.ID)
                {
                    return Unauthorized();
                }

                if (ModelState.IsValid)
                {
                    _registrationRepository.UpdateUser(registration);
                    return RedirectToAction("UserHome", "Restaurant");
                }

                return View(registration);
            }
            catch (Exception ex)
            {
                ViewData["ErrorMessage"] = ex.Message;
                return View("MyError");
            }
        }

        // Edit Admin
        public IActionResult EditAdmin()
        {
            try
            {
                int? userId = HttpContext.Session.GetInt32("UserID");
                if (userId == null)
                {
                    return RedirectToAction("Login", "Restaurant");
                }

                var admin = _registrationRepository.GetUserByID(userId.Value);
                if (admin == null || admin.UserType != "admin")
                {
                    return NotFound();
                }

                return View(admin);
            }
            catch (Exception ex)
            {
                ViewData["ErrorMessage"] = ex.Message;
                return View("MyError");
            }
        }
        // POST: Edit Admin
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditAdmin(Registration registration)
        {
            try
            {
                int? userId = HttpContext.Session.GetInt32("UserID");
                if (userId == null || userId != registration.ID)
                {
                    return Unauthorized();
                }

                if (ModelState.IsValid)
                {
                    _registrationRepository.UpdateUser(registration);
                    return RedirectToAction("AdminHome");
                }

                return View(registration);
            }
            catch (Exception ex)
            {
                ViewData["ErrorMessage"] = ex.Message;
                return View("MyError");
            }
        }
        // POST: Delete User
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            try
            {
                _registrationRepository.DeleteUser(id);
                return RedirectToAction("AllUsers", "Restaurant");
            }
            catch (Exception ex)
            {
                ViewData["ErrorMessage"] = ex.Message;
                return View("MyError");
            }
        }
        //Category Controllers
        //Insert Category
        public IActionResult InsertCategory()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult InsertCategory(Category category, IFormFile? logo)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (logo != null && logo.Length > 0)
                    {
                        using (var memoryStream = new MemoryStream())
                        {
                            logo.CopyTo(memoryStream);
                            category.Logo = memoryStream.ToArray(); 
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("", "Logo file is required.");
                        return View(category);
                    }

                    var result = _categoryRepository.InsertCategory(category);

                    if (result > 0)
                    {
                        return RedirectToAction("ViewAllCategory");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Failed to insert category.");
                    }
                }
            }
            catch (Exception ex)
            {
                ViewData["ErrorMessage"] = ex.Message;
                return View("MyError");
            }

            return View(category);
        }
        //Display all the category
        public IActionResult ViewAllCategory()
        {
            try
            {
            var categories = _categoryRepository.GetAllCategories();
            return View(categories);
        }
        catch (Exception ex)
            {
                ViewData["ErrorMessage"] = ex.Message;
                return View("MyError");
            }
        }
        //Edit Category
        [HttpGet]
        public IActionResult CategoryEdit(int id)
        {
            var category = _categoryRepository.GetCategoryById(id);
            if (category == null)
            {
                return NotFound(); 
            }

            return View(category);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CategoryEdit(Category category, IFormFile? Logo)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (Logo != null && Logo.Length > 0)
                    {
                        using (var memoryStream = new MemoryStream())
                        {
                            Logo.CopyTo(memoryStream);
                            category.Logo = memoryStream.ToArray(); 
                        }
                    }

                    var result = _categoryRepository.UpdateCategory(category);

                    if (result > 0)
                    {
                        return RedirectToAction("ViewAllCategory"); 
                    }
                    else
                    {
                        ModelState.AddModelError("", "Failed to update category.");
                    }
                }
            }
            catch (Exception ex)
            {
                ViewData["ErrorMessage"] = ex.Message;
                return View("MyError");
            }

            return View(category); 
        }
        //Delete a Category by Admin
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteCategory(int id)
        {
            try
            {
                _dishRepository.DeleteAllDishByCategory(id);
                var result = _categoryRepository.DeleteCategory(id);

                if (result > 0)
                {
                    return RedirectToAction("ViewAllCategory"); 
                }
                else
                {
                    ModelState.AddModelError("", "Failed to delete category.");
                }
            }
            catch (Exception ex)
            {
                ViewData["ErrorMessage"] = ex.Message;
                return View("MyError");
            }

            return RedirectToAction("ViewAllCategory"); 
        }
        //Insert Dishes
        [HttpGet]
        public IActionResult InsertDish()
        {
            ViewBag.Categories = _categoryRepository.GetAllCategories();
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult InsertDish(Dish dish)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _dishRepository.InsertDish(dish);
                    return RedirectToAction("InsertDish");  
                }
            }
            catch (Exception ex)
            {
                ViewData["ErrorMessage"] = ex.Message;
                return View("MyError");
            }
            return View(dish);
        }
        //View Dish By Category
        public IActionResult ViewDishByCategory(int id)
        {
            try
            {
                var dishList = _dishRepository.GetDishesByCategoryId(id);
                var category=_categoryRepository.GetCategoryById(id);
                if (dishList == null || !dishList.Any())
                {
                    ViewData["ErrorMessage"] = "No dishes found for this category.";
                    return View("MyError");
                }
                ViewBag.CategoryName = category.CategoryName;
                return View(dishList);
            }
            catch (Exception ex)
            {
                ViewData["ErrorMessage"] = ex.Message;
                return View("MyError");
            }
        }
        public IActionResult EditDish(int id)
        {
            try
            {
                var dish = _dishRepository.GetDishById(id);
                if (dish == null)
                {
                    ViewData["ErrorMessage"] = "Dish not found.";
                    return View("MyError");
                }

                ViewBag.Categories = _categoryRepository.GetAllCategories() ?? new List<Category>();

                return View(dish);
            }
            catch (Exception ex)
            {
                ViewData["ErrorMessage"] = ex.Message;
                return View("MyError");
            }
        }
        [HttpPost]
        public IActionResult EditDish(Dish dish)
        {
            try
            {
                _dishRepository.EditDishDetails(dish);
                return RedirectToAction("ViewAllCategory");
            }
            catch (Exception ex)
            {
                ViewData["ErrorMessage"] = ex.Message;
                return View("MyError");
            } 
        }
        //Delete a Dish
        [HttpPost]
        public IActionResult DeleteDish(int id)
        {
            try
            {
                _dishRepository.DeleteDish(id);
                return RedirectToAction("ViewAllCategory");
            }
            catch (Exception ex)
            {
                ViewData["ErrorMessage"] = ex.Message;
                return View("MyError");
            }
        }
        //User Side Category View
        public IActionResult UserCategoryView()
        {
            try
            {
                var categories = _categoryRepository.GetAllCategories();
                return View(categories);
            }
            catch (Exception ex)
            {
                ViewData["ErrorMessage"] = ex.Message;
                return View("MyError");
            }
        }
        //User side Dishes View
        public IActionResult UserViewDish(int id)
        {
            try
            {
                var dishList = _dishRepository.GetDishesByCategoryId(id);
                var category = _categoryRepository.GetCategoryById(id);
                if (dishList == null || !dishList.Any())
                {
                    ViewData["ErrorMessage"] = "No dishes found for this category.";
                    return View("MyError");
                }
                ViewBag.CategoryName = category.CategoryName;
                return View(dishList);
            }
            catch(Exception ex) 
            {
                ViewData["ErrorMessage"] = ex.Message;
                return View("MyError");
            }
        }
        //Reservation 
        public IActionResult CreateReservation()
        {           
            int userId = HttpContext.Session.GetInt32("UserID") ?? 0;

            if (userId == 0)
            {
                return RedirectToAction("Login", "Restaurant");
            }
            var tables = _addTableRepository.GetTablesByTypeNONAC();
            ViewBag.NonACTables = tables;
            ViewBag.UserID = userId;

            return View();
        }
        //Post Reservation Details
        [HttpPost]
        public IActionResult CreateReservation(Reservation reservation)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    reservation.ID = HttpContext.Session.GetInt32("UserID") ?? 0;

                    if (reservation.ID == 0)
                    {
                        return RedirectToAction("Login", "Restaurant");
                    }

                    bool reservationExists = _reservationRepository.GetReservationsByCriteria(reservation);
                    if (reservationExists)
                    {
                        return RedirectToAction("TableReservationExists");
                    }

                    return View("Payment", reservation);
                }

                return View(reservation);
            }
            catch (Exception ex)
            {
                ViewData["ErrorMessage"] = ex.Message;
                return View("MyError");
            }
        }
        public IActionResult CreateReservationAC()
        {
            int userId = HttpContext.Session.GetInt32("UserID") ?? 0;

            if (userId == 0)
            {
                return RedirectToAction("Login", "Restaurant");
            }
            var actables = _addTableRepository.GetTablesByTypeAC();
            ViewBag.ACTables = actables;
            ViewBag.UserID = userId;

            return View();
        }
        public IActionResult Payment(Reservation reservation)
        {
            return View(reservation);
        }
        [HttpPost]
        public IActionResult ConfirmPayment(Reservation reservation, decimal PaymentAmount)
        {
            try
            {
                if (PaymentAmount == reservation.Amount)
                {
                    _reservationRepository.AddReservation(reservation); 
                    return RedirectToAction("UserReservationStatus");
                }

                ViewBag.ErrorMessage = "The entered amount does not match the required amount.";
                return View("Payment", reservation);
            }
            catch (Exception ex)
            {
                ViewData["ErrorMessage"] = ex.Message;
                return View("MyError");
            }
        }
        //View for Reservation Already Exists
        public IActionResult TableReservationExists()
        {
            return View();
        }
        //Admin see pending Reservations
        public IActionResult AdminGetPendingReservations()
        {
            try
            {
                var reservations = _reservationRepository.GetPendingReservations();
                return View(reservations);
            }
            catch (Exception ex)
            {
                ViewData["ErrorMessage"] = ex.Message;
                return View("MyError");
            }
        }
        //Admin Approve Reservation
        [HttpPost]
        public IActionResult AdminApproveReservation(int reserveId)
        {
            try
            {
                _reservationRepository.ApproveReservation(reserveId);
                return RedirectToAction("AdminGetPendingReservations");
            }
            catch (Exception ex)
            {
                ViewData["ErrorMessage"] = ex.Message;
                return View("MyError");
            }
        }
        // Admin reject a reservation
        [HttpPost]
        public IActionResult AdminRejectReservation(int reserveId)
        {
            try
            {
                _reservationRepository.RejectReservation(reserveId);
                return RedirectToAction("AdminGetPendingReservations");
            }
            catch (Exception ex)
            {
                ViewData["ErrorMessage"] = ex.Message;
                return View("MyError");
            }
        }
        public IActionResult AdminViewReservationStatusHistory()
        {
            try
            {
                var reservations = _reservationRepository.AdminViewReservationStatusHistory();
                return View(reservations);
            }
            catch (Exception ex)
            {
                ViewData["ErrorMessage"] = ex.Message;
                return View("MyError");
            }
        }
        //User see Reservation Status or History
        public IActionResult UserReservationStatus()
        {
            try
            {
                int? userId = HttpContext.Session.GetInt32("UserID");

                if (userId == null)
                {
                    return RedirectToAction("Login", "Restaurant");
                }
                var reservations = _reservationRepository.GetUserReservationStatus(userId.Value);

                return View(reservations);
            }
            catch (Exception ex)
            {
                ViewData["ErrorMessage"] = ex.Message;
                return View("MyError");
            }
        }
        //User Cancels the reservation
        [HttpPost]
        public IActionResult UserCancelReservation(int reserveId)
        {
            try
            {
                _reservationRepository.CancelReservation(reserveId);
                return RedirectToAction("UserReservationStatus");
            }
            catch (Exception ex)
            {
                ViewData["ErrorMessage"] = ex.Message;
                return View("MyError");
            }
        }
        public IActionResult MyError()
        {
            ViewData["ErrorMessage"] = "An unexpected error occurred.";
            return View();
        }

        public IActionResult AdminAddTable()
        {
            return View(new AddTable());
        }

        // POST: AddTable/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AdminAddTable(AddTable model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _addTableRepository.InsertAddTable(model.TableName, model.TableType, model.Amount);
                    return RedirectToAction("AdminViewTable"); 
                }
                catch (Exception ex)
                {
                    ViewData["ErrorMessage"] = ex.Message;
                    return View("MyError");
                }
            }
            return View(model); 
        }
        public IActionResult AdminViewTable()
        {
            var tables = _addTableRepository.GetAllAddTables();
            return View(tables);
        }
        [HttpPost]
        public IActionResult AdminDeleteTable(int tableId)
        {
            try
            {
                _addTableRepository.DeleteAddTable(tableId);
            }
            catch (Exception ex)
            {
                ViewData["ErrorMessage"] = ex.Message;
                return View("MyError");
            }
            return RedirectToAction("AdminViewTable");
        }
        public IActionResult AdminEditTable(int tableId)
        {
            var table = _addTableRepository.GetAddTableById(tableId);
            if (table == null)
            {
                TempData["Error"] = "Table not found!";
                return RedirectToAction("AdminViewTable");
            }
            return View(table);
        }
        [HttpPost]
        public IActionResult AdminEditTable(AddTable model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _addTableRepository.EditAddTable(model);
                    TempData["Message"] = "Table updated successfully!";
                    return RedirectToAction("AdminViewTable");
                }
                catch (Exception ex)
                {
                    ViewData["ErrorMessage"] = ex.Message;
                    return View("MyError");
                }
            }
            return View(model);
        }
    }
}

