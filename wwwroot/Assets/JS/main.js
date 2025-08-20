
/********************************
 * Dealing With Login 
 ********************************/



$(document).ready(function () {

    $("#btnlogin").click(function () {

        const username = $("#username").val();
        const password = $("#password").val();

        $.ajax({
            type: 'POST',
            url: '/Login/Authenticate',
            data: {
                username: username,
                password: password
            },
            success: function (response) {
                if (response.success) {
                    const permission = response.permission; 

                    // Store it for use on other pages
                    localStorage.setItem("permission", permission);

                    window.location.href = "/Pages/Dashboard.html";
                } else {
                    $("#message").text("اسم المستخدم او كلمة السر غير صحيحة").css("color", "red");
                }
            },
            error: function () {
                $("#message").text("Error contacting server").css("color", "orange");
            }
        });
    }); 
}); 

$(document).ready(function () {

    const permission = parseInt(localStorage.getItem("permission"));
 

    if (permission === 1) {
        $("#nav-bar").removeClass("none");
        $("#toggle").removeClass("none");
       
    } else {
        console.log(permission)
        const btnLogout = ` <a href="../index.html"  class="btn btn-outline-light ms-3 fs-3">Logout</a>`; 
        $(".header").append(btnLogout)
        $("#nav-bar").addClass("none");
        $("#toggle").addClass("none");
    }



});

// عند الدخول إلى أي صفحة محمية مثل dashboard.html
$(document).ready(function () {
    const permission = localStorage.getItem("permission");
    if (!permission) {
        window.location.replace("../index.html"); // رجوع للصفحة الرئيسية
    }

    // منع الرجوع بزر back
    history.pushState(null, null, location.href);
    window.onpopstate = function () {
        history.go(1); // المستخدم يرجع لنفس الصفحة
    };
});


 /*******************************
 *  Header Menu
 ********************************/
function toggleMenu() {
    document.getElementById('nav-bar').classList.toggle('active');
}

// Handle dropdowns in mobile
document.addEventListener('DOMContentLoaded', () => {

    const dropdownLinks = document.querySelectorAll('.dropdown > a');

    dropdownLinks.forEach(link => {
        link.addEventListener('click', e => {
            if (window.innerWidth <= 768) {
                e.preventDefault();
                link.parentElement.classList.toggle('active');
            }
        });
    });
});


/*******************************
 * Dealing With Dashboard 
 * 
 ******************************/

$(document).ready(function () {

  
   

    $("#recordCountSelect").change(function () {

        const selected = $(this).val();

        const $rows = $("#table-dashboard").children("tr");

        $rows.hide();

        if (selected === 'All') {
            $rows.show();
        }
        else {
            const count = parseInt(selected);
            $rows.slice(0, count).show();
        }

    });

 
    loadInfoCars(); 

});

function loadInfoCars() {

    $.ajax({
        type: 'GET',
        url: '/Home/Get_infocars',
        success: function (Cars) {
            $("#table-dashboard").empty(); 
            $.each(Cars,function (i, Car) {

                $("#table-dashboard").append(`
                   
                      <tr>
                          <td>${Car.chassisNumber}</td>
                          <td>${Car.carName}</td>
                          <td>${Car.status}</td>
                          <td>${Car.carPrice}</td>
                          <td><button class='btn btn-warning fs-4' onclick="detailsCosts('${Car.chassisNumber}', '${Car.carName}','${Car.sumCosts}')">${Car.sumCosts}</button></td>
                          <td>${Car.profit}</td>
                          </tr>
                `)

            });
            $("#recordCountSelect").val("5").trigger("change");
            $("#recordCountSelect").val("5").trigger("change");
        },
        error: function () {

        }

    });



}

function detailsExpenses(chassisNumber, carName, sumExpenses) {

    if (sumExpenses == 0) {
        return;
    }

    $.ajax({
        type: 'GET',
        url: '/Home/Get_ExpensesCar',
        data: { ChassisNumber: chassisNumber },
        success: function (Expenses) {

            $("#ExpensesCarName").text(carName); 

            $("#dash-detailsExpenses").empty(); 
            $.each(Expenses, function (i, Expense) {

                $("#dash-detailsExpenses").append(`

                   <tr>
                      <td>${Expense.expenseId}</td>
                      <td>${Expense.expenseType}</td>
                      <td>${Expense.amount}</td>
                      <td>${Expense.expenseDate}</td>
                      <td>${Expense.description}</td>
                   </tr>
                `)

            });


            // Show modal
            $("#detailsExpenseModal").modal("show");
        },
        error: function (response) {

        }
    });

}

function detailsCosts(chassisNumber,carName,sumCosts) {

    if (sumCosts == 0) {
        return;
    }
    $.ajax({
        type: 'GET',
        url: '/Home/Get_CostsCar',
        data: { ChassisNumber: chassisNumber },
        success: function (Costs) {

            // Populate form fields
            $("#CostsCarName").text(carName)

            $("#dash-detailsCosts").empty();
            $.each(Costs, function (i, cost) {

                $("#dash-detailsCosts").append(`
                    <tr>
                       <td>${cost.costId}</td>
                       <td>${cost.costName}</td>
                       <td>${cost.costValue}</td>
                    </tr>
            `)
                // Show modal
                $("#detailsCostsModal").modal("show");
            });


        },

        error: function (response) {

        }
    });
}

function Get_AvailabeCars() {

    // Available Car
    $.ajax({
        type: 'GET',
        url: '/Cars/Get_ReportCars',
        data: { condition: "Available" },
        success: function (response) {
            $("#availableCars").text(response);
        },
        error: function () { },

    });
}

function Get_CountUsers() {

    $.ajax({
        type: 'GET',
        url: '/Users/Get_CountUsers',
        success: function (response) {
            $("#CountUsers").text(response); 
        },
        error: function () {

        }


    }); 
}

function Get_SoldCars() {
    // Sold Cars 
    $.ajax({
        type: 'GET',
        url: '/Cars/Get_ReportCars',
        data: { condition: "Sold" },
        success: function (response) {
            $("#carsSold").text(response);
        },
        error: function () { },

    });
}

function Get_InMaintenanceCars() {
    // In Maintenance
    $.ajax({
        type: 'GET',
        url: '/Cars/Get_ReportCars',
        data: { condition: "In Maintenance" },
        success: function (response) {
            $("#carsInMaintenance").text(response);
        },
        error: function () { },

    });
}

function Get_TotalSales() {

    $.ajax({
        type: 'GET',
        url: '/Sales/Get_TotalSales',
        success: function (response) {
            $("#totalSales").text(response);

        },
        error: function () { }

    });
}

function Get_TotalExpenses() {

    $.ajax({
        type: 'GET',
        url: '/Expenses/Get_TotalExpenses',
        success: function (response) {
            $("#totalExpenses").text(response);

        },
        error: function () { }

    }); 
}

function Get_TotalCosts() {

    $.ajax({
        type: 'GET',
        url: '/Costs/Get_TotalCosts',
        success: function (response) {
            $("#totalCost").text(response);
        },
        error: function () { }



    }); 
}

$(document).ready(function () {

    $("#totalSales").off().on("click", function () {

        const permission = parseInt(localStorage.getItem("permission"));

        if (permission === 2) {
          
            window.location.href = "/Pages/SalesResult.html";
      
        }


    });

}); 






    /*******************************
     * Dealing With Cars 
     *******************************/

let allCars = [];
let currentPage = 1;
const rowsPerPage = 10;

function renderTable() {
    const start = (currentPage - 1) * rowsPerPage;
    const end = start + rowsPerPage;
    const pageCars = allCars.slice(start, end);

    const tbody = $('#car-table-body');
    tbody.empty();

    pageCars.forEach(car => {
        tbody.append(`
      <tr>
        <td>${car.chassisNumber}</td>
        <td>${car.carName}</td>
        <td>${car.carType}</td>
        <td>${car.carModel}</td>
        <td>${car.carPrice}</td>
        <td>${car.status}</td>
        <td>${car.supplierName}</td>
        <td>
          <button class="btn  btn-primary" onclick="Get_CarById(${car.chassisNumber})">Update</button>
          <button class="btn  btn-danger" onclick="DeleteCar(${car.chassisNumber}, '${car.carName}')">Delete</button>
        </td>
      </tr>
    `);
    });
}
function renderPaginationTabs() {
    const pageCount = Math.ceil(allCars.length / rowsPerPage);
    const tabList = $('#pagination-tabs');
    tabList.empty();

    for (let i = 1; i <= pageCount; i++) {
        const active = i === currentPage ? 'active' : '';
        tabList.append(`
      <li class="nav-item">
        <a class="btn btn-primary me-1 ${active}" href="#" onclick="goToPage(${i})">Page ${i}</a>
      </li>
    `);
    }
}

function goToPage(page) {
    currentPage = page;
    renderTable();
    renderPaginationTabs();
}

$(document).ready(function () {
    loadCars();
}); 
function loadCars() {

    $.ajax({
        type: 'Get',
        url: '/Cars/Get_AllCars',
        success: function (Cars) {
            allCars = Cars;
            currentPage = 1;
            renderTable();
            renderPaginationTabs();

        },
        error: function () {
            alert('Failed to laod Cars'); 
        }
    });
}

function Get_CarById(ChassisNumber) {
            

        $.ajax({
            type: 'GET',
            url: '/Cars/Get_CarById',
            data: { chassisNumber : ChassisNumber },
            success: function (response) {

                const Car = response[0]; 

                $("#CarId").val(Car.chassisNumber);
                $("#CarName").val(Car.carName);
                $("#CarType").val(Car.carType);
                $("#CarModel").val(Car.carModel);
                $("#CarPrice").val(Car.carPrice);
                $("#edit-CarStatus").val(Car.status);
                $("#CarSupplier").val(Car.supplierName);

                $('#editCarModal').modal('show'); 

            
            },
            error: function () {
                console.log("error");
            }
        }); 
    }

$("#btn-addCar").on("click", function () {

    $("#addCarModal").modal("show"); 

    //Get Data Supplier
    $.ajax({
        type: 'GET',
        url: '/Suppliers/GetSupplierToList/',
        success: function (data) {
            const $dropdown = $("#add-SupplierId");
            $dropdown.empty().append('<option value="">-- Select Supplier --</option>');
            $.each(data, function (i, supplier) {
                $dropdown.append(`<option value="${supplier.supplier_Id}">${supplier.companyName}</option>`);
            });
        },
        error: function () {
            alert("Failed to load suppliers.");
        }
    });
  
});

$("#AddCar").off().on("click", function () {

    const chassisNumber = $("#add-CarId").val()
    const carName = $("#add-CarName").val();
    const carType = $("#add-CarType").val();
    const carModel = $("#add-CarModel").val();
    const carPrice = $("#add-CarPrice").val();
    const carStatus = $("#add-CarStatus").val();
    const supplierId = $("#add-SupplierId").val();

    $("#AddMessage").hide();

    $.ajax({
        type: 'POST',
        url: '/Cars/AddCar/',
        data: {
            ChassisNumber: chassisNumber,
            CarName: carName,
            CarType: carType,
            CarModel: carModel,
            CarPrice: carPrice,
            CarStatus: carStatus,
            SupplierId:supplierId
        },
        success: function (response) {
            if (response.success) {
                $("#AddMessage").text("Car Added successfully!").show();
                setTimeout(() => {
                    $("#AddCarModal").modal("hide");
                    location.reload();
                }, 1000);
            } else {
                $("#AddMessage").text("Add failed!").show();
            }
        },
        error: function () {
            $("#AddMessage").text("Server error during update!").show();
        }


    });


});

$("#editCar").off().on("click", function () {

    const chassisNumber = $("#CarId").val()
    const carName = $("#CarName").val();
    const carType = $("#CarType").val();
    const carModel = $("#CarModel").val();
    const carPrice = $("#CarPrice").val();
    const carStatus = $("#edit-CarStatus").val();

    // $("#CarSupplier").val(Car.supplierName);

    $("#EditMessage").hide();

    $.ajax({
        type: 'POST',
        url: '/Cars/UpdateCar/',
        data: {
            ChassisNumber: chassisNumber,
            CarName: carName,
            CarType: carType,
            CarModel: carModel,
            CarPrice: carPrice,
            CarStatus: carStatus
        },
        success: function (response) {
            if (response.success) {
                $("#EditMessage").text("Car Updated successfully!").show();
                setTimeout(() => {
                    $("#editCarModal").modal("hide");
                 //    location.reload();
                }, 1000);
            } else {
                $("#EditMessage").text("Update failed!").show();
            }
        },
        error: function () {
            $("#EditMessage").text("Server error during update!").show();
        }


    });


});

function DeleteCar(ChassisNumber, CarName) {

    $("#infoCar").text(ChassisNumber + " " + CarName);

    $("#deleteMessage").hide(); 

    $("#deleteCarModal").modal("show");

    $("#confirmDeleteCarBtn").off("click").on("click",function () {

        $.ajax({
            type: 'POST',
            url: '/Cars/DeleteCar',
            data: { chassisNumber: ChassisNumber },
            success: function () {
                $("#deleteMessage").text("Car deleted successfully!").show();

                setTimeout(() => {
                    $("#deleteCarModal").modal("hide");
                    location.reload(); 
                }, 1000);
               
            },
            error: function () {
                $("#deleteMessage").text("Car deleted Failed!").show();
            }

        }); 


    });



   


};



/*******************************
 *  Dealing With Users
 *******************************/

$(document).ready(function () {
   LoadUsers();
});

function LoadUsers() {

    $.ajax({
        type: 'GET',
        url: '/Users/Get_AllUsers',
        success: function (Users) {
            $('#user-table-body').empty();
            $.each(Users, function (i, user) {

                $('#user-table-body').append(`
                    
                   <tr>
                      <td>${user.user_Id}</td>
                      <td>${user.username}</td>
                      <td>${user.password}</td>
                      <td>${user.permissions}</td>
                      <td>                
                        <button class="btn btn-sm btn-primary fs-5" onclick="Get_UserById(${user.user_Id})">Update</button>
                        <button class="btn btn-sm btn-danger fs-5" onclick="DeleteUser(${user.user_Id},'${user.username}')">Delete</button>
                      </td>
                  </tr>`);


            });
        },
        error: function () {
            alert('Failed to load Users')
        }


    });





};

function Get_UserById(user_Id) {


    $.ajax({
        type: 'GET',
        url: '/Users/Get_UserById',
        data: { userId : user_Id },
        success: function (response) {
            const user = response[0]; 

            $("#UserId").val(user.userId); 
            $("#userName").val(user.username); 
            $("#password").val(user.password); 
            $("#UserPermissions").val(user.permissions); 

            $("#editUserModal").modal("show"); 

        },
        error:function() {}
    }); 
}

$("#btn-AddUser").on("click", function () {


    // Get next userId
    $.ajax({
        type: 'GET',
        url: '/Users/Get_NextUserId',
        success: function (response) {
            $("#add-userId").val(response.nextUserId)
        },
        error: function () {
            alert("Failed to Get userId.");
        }
    });

    //Get Data Permissions 
    $.ajax({
        type: 'GET',
        url: '/UserPermissions/GetPermissionsToList/',
        success: function (data) {
            const $dropdown = $("#add-permissions");
            $dropdown.empty().append('<option value="">-- Select Permissions --</option>');
            $.each(data, function (i, Permission) {
                $dropdown.append(`<option value="${Permission.per_Id}">${Permission.perGroup}</option>`);
            });
        },
        error: function () {
            alert("Failed to load Permissions.");
        }
    });

    $("#addUserModal").modal("show");

});

$("#AddUser").off().on("click", function () {

    const userId = $("#add-userId").val()
    const userName = $("#add-username").val();
    const password = $("#add-password").val();
    const permission = $("#add-permissions").val();

    $("#AddMessage").hide();

    $.ajax({
        type: 'POST',
        url: '/Users/AddUser/',
        data: {
            UserId: userId,
            UserName: userName,
            Password: password,
            Per_Id: permission,
        },
        success: function (response) {
            if (response.success) {
                $("#AddMessage").text("User Added successfully!").show();
                setTimeout(() => {
                    $("#AddCarModal").modal("hide");
                    location.reload();
                }, 1000);
            } else {
                $("#AddMessage").text("Add failed!").show();
            }
        },
        error: function () {
            $("#AddMessage").text("Server error during update!").show();
        }


    });


});

$("#editUser").off().on("click", function () {

    const userId = $("#UserId").val()
    const userName = $("#userName").val();
    const password = $("#password").val();

    $("#EditMessage").hide();

    $.ajax({
        type: 'POST',
        url: '/Users/UpdateUser',
        data: {
            UserId: userId,
            UserName: userName,
            Password: password,
        },
        success: function (response) {
            if (response.success) {
                $("#EditMessage").text("User Updated successfully!").show();
                setTimeout(() => {
                    $("#editUserModal").modal("hide");
                    location.reload();
                }, 1000);
            } else {
                $("#EditMessage").text("Update failed!").show();
            }
        },
        error: function () {
            $("#EditMessage").text("Server error during update!").show();
        }


    });


});
function DeleteUser(UserId, UserName) {

    $("#infoUser").text(UserId + " " + UserName); 
   
    $("#deleteMessage").hide();

    $("#deleteUserModal").modal("show");

    $("#confirmDeleteUserBtn").off("click").on("click", function () {

        $.ajax({
            type: 'POST',
            url: '/Users/DeleteUser',
            data: { userId: UserId },
            success: function () {
                $("#deleteMessage").text("User deleted successfully!").show();

                setTimeout(() => {
                    $("#deleteUserModal").modal("hide");
                    location.reload();
                }, 1000);

            },
            error: function () {
                $("#deleteMessage").text("User deleted Failed!");
            }

        });


    });

};


/****************************
 * Dealing With Customers  
 ****************************/

$(document).ready(function () {
    LoadCustomers();
}); 
function LoadCustomers() {

    $.ajax({
        type: 'GET',
        url: '/Customers/Get_AllCustomers',
        success: function (Customers) {
            $("#customer-table-body").empty();
            $.each(Customers, function (i, cust) {

                $("#customer-table-body").append(`
                  
                   <tr>
                       <td>${cust.cust_Id}</td>
                       <td>${cust.fullName}</td>
                       <td>${cust.phone}</td>
                       <td>${cust.email}</td>
                       <td>${cust.address}</td>
                       <td>${cust.nationalID}</td>
                       <td>
                            <button class="btn btn-sm btn-primary fs-5" onclick="Get_CusteomerById(${cust.cust_Id})">Update</button>
                            <button class="btn btn-sm btn-danger fs-5" onclick="DeleteCustomer(${cust.cust_Id}, '${cust.fullName}')">Delete</button>
                      </td>

                   </tr>

                `)


            });

        },
        error: function () {
            alert('Failed to load Customers');
        }



    }); 


}

function Get_CusteomerById(Cust_Id) {

    $.ajax({
        type: 'GET',
        url: '/Customers/Get_CustomerById',
        data: { custId: Cust_Id },
        success: function (response) {
            const Cust = response[0];

            $("#CustId").val(Cust.custId);
            $("#CustName").val(Cust.fullName);
            $("#CustPhone").val(Cust.phone);
            $("#CustEmail").val(Cust.email);
            $("#CustAddress").val(Cust.address);
            $("#CustNationalId").val(Cust.nationalID);
         

            $("#editCustModal").modal("show");

        },
        error: function () { }
    }); 


}

$("#btn-addCustomer").on("click", function () {

    // Get next custId
    $.ajax({
        type: 'GET',
        url: '/Customers/Get_NextCustId',
        success: function (response) {
            $("#add-custId").val(response.nextCustId);
        },
        error: function () {
            alert("Failed to Get CustomerId.");
        }
    });

    $("#addCustomerModal").modal("show");
});

$("#AddCustomer").off().on("click", function () {

    const custId = $("#add-custId").val()
    const custname = $("#add-custname").val();
    const custphone = $("#add-custphone").val();
    const custemail = $("#add-custemail").val();
    const custaddress = $("#add-custaddress").val();
    const custnationalid = $("#add-custnationalid").val();

    $("#AddMessage").hide();

    $.ajax({
        type: 'POST',
        url: '/Customers/AddCustomer',
        data: {
            CustId: custId,
            Custname: custname,
            Custphone: custphone,
            Custemail: custemail,
            Custaddress: custaddress,
            Custnationalid: custnationalid
        },
        success: function (response) {
            if (response.success) {
                $("#AddMessage").text("Cusomter Added successfully!").show();
                setTimeout(() => {
                    $("#AddCarModal").modal("hide");
                    location.reload();
                }, 1000);
            } else {
                $("#AddMessage").text("Add failed!").show();
            }
        },
        error: function () {
            $("#AddMessage").text("Server error during update!").show();
        }


    });


});

$("#editCustomer").off().on("click", function () {

    const custId = $("#CustId").val()
    const custName = $("#CustName").val();
    const custPhone = $("#CustPhone").val();
    const custEmail = $("#CustEmail").val();
    const custAddress = $("#CustAddress").val();
    const custNationalId = $("#CustNationalId").val();

    $("#EditMessage").hide();

    $.ajax({
        type: 'POST',
        url: '/Customers/UpdateCustomer',
        data: {
            CustId: custId,
            CustName: custName,
            CustPhone: custPhone,
            CustEmail: custEmail,
            CustAddress: custAddress,
            CustNationalId: custNationalId,
        },
        success: function (response) {
            if (response.success) {
                $("#EditMessage").text("Customer Updated successfully!").show();
                setTimeout(() => {
                    $("#editCustomerModal").modal("hide");
                    location.reload();
                }, 1000);
            } else {
                $("#EditMessage").text("Update failed!").show();
            }
        },
        error: function () {
            $("#EditMessage").text("Server error during update!").show();
        }


    });


});
function DeleteCustomer(CustId, CustomerName) {

    $("#infoCustomer").text(CustId + " " + CustomerName); 
   
    $("#deleteMessage").hide();

    $("#deleteCustomerModal").modal("show");

    $("#confirmDeleteCustomerBtn").off("click").on("click", function () {

        $.ajax({
            type: 'POST',
            url: '/Customers/DeleteCustomer',
            data: { custId: CustId },
            success: function () {
                $("#deleteMessage").text("Customer deleted successfully!").show();

                setTimeout(() => {
                    $("#deleteCustomerModal").modal("hide");
                    location.reload();
                }, 1000);

            },
            error: function () {
                $("#deleteMessage").text("Customer deleted Failed!");
            }

        });


    });

};


/******************************
 *  Dealing With Suppliers
 ******************************/

$(document).ready(function () {
    LoadSuppliers();
});

function LoadSuppliers() {

    $.ajax({
        type: 'GET',
        url: '/Suppliers/Get_AllSuppliers',
        success: function (Suppliers) {
            $('#supplier-table-body').empty();
            $.each(Suppliers, function (i, sup) {

                $('#supplier-table-body').append(`
                    
                   <tr>
                      <td>${sup.supplier_Id}</td>
                      <td>${sup.companyName}</td>
                      <td>${sup.phone}</td>
                      <td>${sup.email}</td>
                      <td>${sup.address}</td>
                      <td>                
                        <button class="btn btn-primary btn-sm fs-5" onclick="Get_SupplierById(${sup.supplier_Id})">Update</button>
                        <button class="btn btn-danger btn-sm fs-5" onclick="DeleteSupplier(${sup.supplier_Id},'${sup.companyName}')">Delete</button>
                      </td>
                  </tr>`);
            });
        },
        error: function () {
            alert('Failed to load Suppliers')
        }


    });

}
function Get_SupplierById(supplier_Id) {

    $.ajax({
        type: 'GET',
        url: '/Suppliers/Get_SupplierById',
        data: { SupplierId: supplier_Id },
        success: function (response) {
            const Supplier = response[0];

            $("#SupplierId").val(Supplier.supplier_Id);
            $("#SupplierName").val(Supplier.companyName);
            $("#SupplierPhone").val(Supplier.phone);
            $("#SupplierEmail").val(Supplier.email);
            $("#SupplierAddress").val(Supplier.address);

            $("#editSupplierModal").modal("show");

        },
        error: function () { }
    });


}

$("#btn-addSupplier").on("click", function () {

    // Get next custId
    $.ajax({
        type: 'GET',
        url: '/Suppliers/Get_NextSupplierId',
        success: function (response) {
            $("#add-supplierId").val(response.nextSupplierId)
        },
        error: function () {
            alert("Failed to Get CustomerId.");
        }
    });

    $("#addSupplierModal").modal("show");
});

$("#AddSupplier").off().on("click", function () {

    const supplierId = $("#add-supplierId").val()
    const suppliername = $("#add-suppliername").val();
    const supplierphone = $("#add-supplierphone").val();
    const supplieremail = $("#add-supplieremail").val();
    const supplieraddress = $("#add-supplieraddress").val();

    $("#AddMessage").hide();

    $.ajax({
        type: 'POST',
        url: '/Suppliers/AddSupplier',
        data: {
            SupplierId: supplierId,
            Suppliername: suppliername,
            Supplierphone: supplierphone,
            Supplieremail: supplieremail,
            Supplieraddress: supplieraddress
        },
        success: function (response) {
            if (response.success) {
                $("#AddMessage").text("Suppliper Added successfully!").show();
                setTimeout(() => {
                    $("#AddCarModal").modal("hide");
                    location.reload();
                }, 1000);
            } else {
                $("#AddMessage").text("Add failed!").show();
            }
        },
        error: function () {
            $("#AddMessage").text("Server error during Adding!").show();
        }


    });


});

$("#editSupplier").off().on("click", function () {

    const supplierId = $("#SupplierId").val()
    const companyName = $("#SupplierName").val();
    const phone = $("#SupplierPhone").val();
    const email = $("#SupplierEmail").val();
    const address = $("#SupplierAddress").val();

    $("#EditMessage").hide();

    $.ajax({
        type: 'POST',
        url: '/Suppliers/UpdateSupplier',
        data: {
            SupplierId: supplierId,
            CompanyName: companyName,
            Phone: phone,
            Email: email,
            Address: address,

        },
        success: function (response) {
            if (response.success) {
                $("#EditMessage").text("Supplier Updated successfully!").show();
                setTimeout(() => {
                    $("#editSupplierModal").modal("hide");
                    location.reload();
                }, 1000);
            } else {
                $("#EditMessage").text("Update failed!").show();
            }
        },
        error: function () {
            $("#EditMessage").text("Server error during update!").show();
        }


    });


});
function DeleteSupplier(supplier_Id, SupplierName) {

    $("#infoSupplier").text(supplier_Id + " " + SupplierName); 
    
    $("#deleteMessage").hide();

    $("#deleteSupplierModal").modal("show");

    $("#confirmDeleteSupplierBtn").off("click").on("click", function () {

        $.ajax({
            type: 'POST',
            url: '/Suppliers/DeleteSupplier',
            data: { supplierId: supplier_Id },
            success: function () {
                $("#deleteMessage").text("Supplier deleted successfully!").show();

                setTimeout(() => {
                    $("#deleteSupplierModal").modal("hide");
                    location.reload();
                }, 1000);

            },
            error: function () {
                $("#deleteMessage").text("User deleted Failed!");
            }

        });


    });

}

/*****************************
 *   Dealing With Sales
 *****************************/

$(document).ready(function () {
    LoadMasterInvoices(); 
});
  
function LoadMasterInvoices()
{
    $.ajax({
        type: 'GET',
        url: '/Sales/Get_MasterInvoices',
        success: function (Masters) {
            $("#MasterInvoice-table-body").empty(); 
            $.each(Masters, function (i, master) {

                const currentPage = window.location.pathname;
                let deleteButton = "";


                if (currentPage.includes("Sales.html")) {
                    deleteButton = `<button class="btn btn-sm btn-danger fs-5 " onclick="DeleteInvoice(${master.inv_Id})">Delete</button>`;
                }


                $("#MasterInvoice-table-body").append(`
                   <tr>
                        <td>${master.inv_Id}</td>
                        <td>${master.inv_Value}</td>
                        <td>${master.inv_date}</td>
                        <td>${master.payment_type}</td>
                        <td>${master.fullName}</td>
                        <td>
                          <button class="btn btn-sm btn-success btn-view fs-5" onclick="ViewDetails(${master.inv_Id})">View Detials</button>
                          <!--
                          <button class="btn btn-sm btn-primary fs-5" onclick="Get_InvoiceById(${master.inv_Id})">Update</button>
                          <button class="btn btn-sm btn-danger fs-5" onclick="DeleteInvoice(${master.inv_Id})" id="delete-sales">Delete</button>
                          -->
                          ${deleteButton}
                        </td>
                   </tr>
                `);

            })
        },
        error: function () {
            alert('Failed to load Master Invoices')
        }


    });

}



function ViewDetails(Inv_id)
    {
 
    $.ajax({
        type: 'GET',
        url: '/Sales/Get_DetailsInvoice',
        data: {
            Inv_id : Inv_id
        },
        success: function (Details) {
            $('#DetailInvoice-table-body').empty(); 
            $.each(Details, function (i,detail) {

                $('#DetailInvoice-table-body').append(`

                  <tr>
                       <td>${detail.inv_Id}</td>
                       <td>${detail.chassisNumber}</td>
                       <td>${detail.carName}</td>
                       <td>${detail.carPrice}</td>
                       <td>${detail.quantity}</td>
                  </tr>
                `);

            });



        },
        error: function () { }

    });

}
function Get_InvoiceById(Inv_Id) {

    $.ajax({
        type: 'GET',
        url: '/Sales/Get_InvoiceById',
        data: { invId: Inv_Id },
        success: function (invoices) {

            if (invoices.length === 0) return;

            const invoice = invoices[0];

            $("#invId").val(invoice.inv_Id); 
            $("#invDate").val(invoice.inv_Date);
            $("#invValue").val(invoice.inv_Value);
            $("#custName").val(invoice.customerName); 


            $("#detailsTableBody").empty();

            // Append each detail row
            invoice.details.forEach(item => {
                $("#detailsTableBody").append(`
            <tr>
                <td>${item.chassisNumber}</td>
                <td>${item.carName}</td>
                <td>${item.carPrice}</td>
                <td>${item.quantity}</td>
            </tr>`);
            });

            $('#EditinvoiceModal').modal('show');

        },
        error: function () {

        }


    });

   
}

function calculateInvoiceValue() {
    let total = 0;

    $("#detailsInvoiceTableBody tr").each(function () {
        const price = parseFloat($(this).find("td:nth-child(3)").text()) || 0;
        const quantity = parseInt($(this).find(".quantity-input").val()) || 0;

        total += price * quantity;
    });

    $("#invValue").val(total.toFixed(2)); // set total in input field
}

$(document).on("input", ".quantity-input", function () {
    calculateInvoiceValue();
});

$("#btn-addInvoice").on("click", function () {

    // Get Number Max Invoice 
    $.ajax({
        type: 'GET',
        url:'/Sales/Get_NextInvoiceId',
        success: function (nextInvoiceId) {
            $("#invId").val(nextInvoiceId); 
        },
        error: function () { }


    });

    // Get Data Customer 
    $.ajax({
        type: 'GET',
        url: '/Customer/Get_CustomerInvoice',
        success: function (Customers) {
            const $dropdown = $("#addInvoice-custName");
            $dropdown.empty().append(`<option>--Select Customer --</option>`);

            $.each(Customers, function (i, cust) {
                $dropdown.append(`<option value="${cust.cust_Id}">${cust.fullName}</option>`);
            });


        },
        error: function () { }

    });

    // Get Data Car
    $.ajax({
        type: 'GET',
        url: '/Cars/GetCarToList/',
        success: function (data) {
            const $dropdown = $("#addInvoice-carId");
            $dropdown.empty().append(`<option>--Select Car--</option>`);
            $.each(data, function (i, car) {
                $dropdown.append(`<option value="${car.chassisNumber}">${car.carName}</option>`);
            });
        },
        error: function () {
            alert("Failed to load suppliers.");
        }
    });

    const now = new Date(); 
    const formattedDate = now.toLocaleDateString("en-GB"); 
    $("#invDate").val(formattedDate);


    $("#AddinvoiceModal").modal("show");

});

$("#addInvoice-carId").on("change", function () {

    const carId = $(this).val(); 

    $.ajax({
        type: 'GET',
        url: '/Cars/Get_CarInvoice',
        data: { ChassisNumber: carId },
        success: function (response) {

            const car = response[0]; 

            if (!car || !car.chassisNumber) return;

            $("#detailsInvoiceTableBody").append(`
                <tr>
                    <td>${car.chassisNumber}</td>
                    <td>${car.carName}</td>
                    <td>${car.carPrice}</td>
                    <td>
                        <input type="number" class="form-control quantity-input" value="1" min="1" style="width: 80px;">
                    </td>
                     <td><button class="btn btn-danger btn-sm delete-row">Delete</button></td>
                </tr>
            `);
            calculateInvoiceValue();
        },
        error: function () {

        }
    });

});

$(document).on("input", ".quantity-input", function () {
    calculateInvoiceValue(); // Recalculate total invoice value
});

$(document).on("click", ".delete-row", function () {
    $(this).closest("tr").remove(); // Remove the row from the table
    calculateInvoiceValue();
});

function formatDateToISO(dateStr) {
    const parts = dateStr.split('/'); // ["06", "07", "2025"]
    // Assuming dd/MM/yyyy
    return `${parts[2]}-${parts[1].padStart(2, '0')}-${parts[0].padStart(2, '0')}`;
}

$("#addInvoice").off().on("click", function () {
    const invDateRaw = $("#invDate").val(); // e.g. "06/07/2025"
    const invDate = formatDateToISO(invDateRaw);

    const data = {
        invId: $("#invId").val(),
        invDate: invDate,
        invValue: $("#invValue").val(),
        paymentType: $("#paymentType").val(),
        CustId: $("#addInvoice-custName").val(),
        detailCount: 0 // we'll update this below
    };

    // Flatten each car detail
    $("#detailsInvoiceTableBody tr").each(function (i) {
        const chassisNumber = $(this).find("td:nth-child(1)").text().trim();
        const carName = $(this).find("td:nth-child(2)").text().trim();
        const carPrice = $(this).find("td:nth-child(3)").text().trim();
        const quantity = $(this).find(".quantity-input").val();

        data[`chassisNumber${i}`] = chassisNumber;
        data[`carName${i}`] = carName;
        data[`carPrice${i}`] = carPrice;
        data[`quantity${i}`] = quantity;
        data.detailCount++;
    });

    if (data.detailCount === 0) {
        alert("Please add at least one car to the invoice.");
        return;
    }

    $.ajax({
        type: "POST",
        url: "/Sales/AddInvoice",
        data: data, // plain object, not JSON.stringify
        success: function (res) {
            if (res.success) {
                setTimeout(function () {
                    $("#AddMessage").text("Invoice saved successfully!");
                    location.reload();
                },1000)
               
            } else {
                $("#AddMessage").text("Error: " + res.message);
            }
        },
        error: function (err) {
            $("#AddMessage").text("Failed to save invoice.");
        }
    });
});

$("#editInvoice").off().on("click", function () {

    const CostId = $("#CostId").val()
    const CostName = $("#CostName").val();
    const CostValue = $("#CostValue").val();
    const ChassisNumber = $("#chassisNumber").val();


    $("#EditMessage").hide();

    $.ajax({
        type: 'POST',
        url: '/Costs/UpdateCost',
        data: {
            costId: CostId,
            costName: CostName,
            costValue: CostValue,
            chassisNumber: ChassisNumber,
        },
        success: function (response) {
            if (response.success) {
                $("#EditMessage").text("Cost Updated successfully!").show();
                setTimeout(() => {
                    $("#editCostModal").modal("hide");
                    location.reload();
                }, 1000);
            } else {
                $("#EditMessage").text("Update failed!").show();
            }
        },
        error: function () {
            $("#EditMessage").text("Server error during update!").show();
        }


    });


});
function DeleteInvoice(Inv_Id) {

    $("#infoInvoice").text(Inv_Id); 

    $("#deleteMessage").hide();

    $("#deleteInvoiceModal").modal("show");

    $("#confirmDeleteInvoiceBtn").off("click").on("click", function () {

        $.ajax({
            type: 'POST',
            url: '/Sales/DeleteInvoice',
            data: { invId: Inv_Id },
            success: function () {
                $("#deleteMessage").text("Invoice deleted successfully!").show();

                setTimeout(() => {
                    $("#deleteInvoiceModal").modal("hide");
                    location.reload();
                }, 1000);

            },
            error: function () {
                $("#deleteMessage").text("Invoice deleted Failed!");
            }

        });


    });

}

/****************************
 *  Dealing With Expenses
 ****************************/

$(document).ready(function () {
    LoadExpenses();
});
function LoadExpenses() {

        $.ajax({
            type: 'GET',
            url: '/Expenses/Get_AllExpenses',
            success: function (Expenses) {
                $('#expense-table-body').empty(); 
                $.each(Expenses, function (i, exp) {
               
                    $('#expense-table-body').append(`
                       <tr>
                          <td>${exp.expense_Id} </td>
                          <td>${exp.expenseType}</td>
                          <td>${exp.amount}</td>
                          <td>${new Date(exp.expenseDate).toLocaleDateString("en-GB")}</td>
                          <td>${exp.description}</td>
                          <td>${exp.carName}</td>
                          <td>
                              <button class="btn btn-sm btn-primary fs-5" onclick="Get_ExpenseById(${exp.expense_Id})">Update</button>
                              <button class="btn btn-sm btn-danger fs-5" onclick="DeleteExpense(${exp.expense_Id},'${exp.expenseType}')">Delete</button>
                          </td>

                       </tr>
                    `);


                });



            },
            error: function () {
                alert('Failed to load Expenses'); 
            }




        });
    }
function Get_ExpenseById(expense_Id) {

    $.ajax({
        type: 'GET',
        url: '/Expenses/Get_ExpenseById',
        data: { expenseId: expense_Id },
        success: function (response) {
            const expense = response[0]; 
            let expDate = new Date(expense.expenseDate).toLocaleDateString("en-GB"); 

            // Populate form fields
            $("#ExpenseId").val(expense.expense_Id);
            $("#ExpenseType").val(expense.expenseType);
            $("#ExpenseAmount").val(expense.amount);
            $("#ExpenseDate").val(expDate);
            $("#ExpenseDescription").val(expense.description);
            $("#ChassisNumber").val(expense.chassisNumber);

            // Show modal
            $("#editExpenseModal").modal("show");
        }

    });
}

$("#btn-addExpense").on("click", function () {
  
 
        // Get next expensesId
        $.ajax({
            type: 'GET',
            url: '/Expenses/Get_NextExpenseId',
            success: function (response) {
                $("#add-expenseId").val(response.nextExpenseId)
            },
            error: function () {
                alert("Failed to load Permissions.");
            }
        }); 

        //Get Data Car
        $.ajax({
            type: 'GET',
            url: '/Cars/GetCarToList/',
            success: function (data) {
                const $dropdown = $("#add-carId");
                $dropdown.empty().append(`<option>--Select Car--</option>`);
                $.each(data, function (i, car) {
                    $dropdown.append(`<option value="${car.chassisNumber}">${car.carName}</option>`);
                });
            },
            error: function () {
                alert("Failed to load suppliers.");
            }
        });

        $("#addExpenseModal").modal("show");

});

$("#AddExpense").off().on("click", function () {

    const expenseId = $("#add-expenseId").val()
    const expenseType = $("#add-expenseType").val();
    const expenseAmount = $("#add-expenseAmount").val();
    const expenseDate = $("#add-expenseDate").val();
    const expenseDescription = $("#add-expenseDescription").val();
    const carId = $("#add-carId").val();


    $("#AddMessage").hide();

    $.ajax({
        type: 'POST',
        url: '/Expenses/AddExpense/',
        data: {
            ExpenseId: expenseId,
            ExpenseType: expenseType,
            ExpenseAmount: expenseAmount,
            ExpenseDate: expenseDate,
            ExpenseDescription: expenseDescription,
            CarId: carId
        },
        success: function (response) {
            if (response.success) {
                $("#AddMessage").text("Expense Added successfully!").show();
                setTimeout(() => {
                    $("#AddExpenseModal").modal("hide");
                  //  location.reload();
                }, 1000);
            } else {
                $("#AddMessage").text("Add failed!").show();
            }
        },
        error: function () {
            $("#AddMessage").text("Server error during Adding!").show();
        }


    });


});

$("#editExpense").off().on("click", function () {

    const expensId = $("#ExpenseId").val()
    const expenseType = $("#ExpenseType").val();
    const expenseAmount = $("#ExpenseAmount").val();
    const expenseDate = $("#ExpenseDate").val();
    const expenseDescription = $("#ExpenseDescription").val();

    $("#EditMessage").hide();

    $.ajax({
        type: 'POST',
        url: '/Expenses/UpdateExpense',
        data: {
            expensId: expensId,
            ExpenseType: expenseType,
            ExpenseAmount: expenseAmount,
            ExpenseDate: expenseDate,
            ExpenseDescription: expenseDescription,

        },
        success: function (response) {
            if (response.success) {
                $("#EditMessage").text("Expense Updated successfully!").show();
                setTimeout(() => {
                    $("#editExpenseModal").modal("hide");
                    location.reload();
                }, 1000);
            } else {
                $("#EditMessage").text("Update failed!").show();
            }
        },
        error: function () {
            $("#EditMessage").text("Server error during update!").show();
        }


    });


});
function DeleteExpense(expense_Id, ExpenseType) {

    $("#infoExpense").text(expense_Id + " " + ExpenseType); 
   
    $("#deleteMessage").hide();

    $("#deleteExpenseModal").modal("show");

    $("#confirmDeleteExpenseBtn").off("click").on("click", function () {

        $.ajax({
            type: 'POST',
            url: '/Expenses/DeleteExpense',
            data: { expenseId: expense_Id },
            success: function () {
                $("#deleteMessage").text("Expense deleted successfully!").show();

                setTimeout(() => {
                    $("#deleteExpenseModal").modal("hide");
                    location.reload();
                }, 1000);

            },
            error: function () {
                $("#deleteMessage").text("Expense deleted Failed!");
            }

        });


    });

}



/******************************
 *  Dealing With Costs
 ******************************/

$(document).ready(function () {
    LoadCosts(); 
});
function LoadCosts() {

    $.ajax({
            type: 'GET',
            url: '/Costs/Get_AllCosts',
            success: function (Costs) {
                $('#cost-table-body').empty();
                $.each(Costs, function (i, cost) {

                    $('#cost-table-body').append(`
                      <tr>
                          <td>${cost.cost_Id} </td>
                          <td>${cost.costName}</td>
                          <td>${cost.costValue}</td>
                          <td>${cost.carName}</td>
                          <td>
                              <button class="btn btn-sm btn-primary fs-5" onclick="Get_CostById(${cost.cost_Id})">Update</button>
                              <button class="btn btn-sm btn-danger fs-5" onclick="DeleteCost(${cost.cost_Id},'${cost.costName}')">Delete</button>
                          </td>

                   </tr>`);


                });
            },
                    error: function () {
                        alert('Failed to load Costs');
                    }
        });


 }
function Get_CostById(cost_Id) {

    $.ajax({
        type: 'GET',
        url: '/Costs/Get_CostById',
        data: { costId: cost_Id },
        success: function (response) {
            const cost = response[0]; 

            $("#CostId").val(cost.cost_Id);
            $("#CostName").val(cost.costName);
            $("#CostValue").val(cost.costValue);
         //   $("#chassisNumber").val(cost.chassisNumber);

            //get Data Car
            $.ajax({
                type: 'GET',
                url: '/Cars/Get_CarToList/',
                data: { CarId: cost.chassisNumber },
                success: function (data) {
                    const $dropdown = $("#editCost-chassisNumber");
                    $dropdown.empty();
                    $.each(data, function (i, car) {
                        $dropdown.append(`<option value="${car.chassisNumber}">${car.carName}</option>`);
                    });
                },
                error: function () {
                    alert("Failed to load Cars.");
                }
            });

            $("#editCostModal").modal("show");

        },

        error: function () {

        }

    });
}

$("#btn-addCost").on("click", function () {

    //Get next costId
    $.ajax({
        type: 'GET',
        url: '/Costs/Get_NextCostId',
        success: function (response) {
            $("#add-costId").val(response.nextCostId)
        },
        error: function () {
            alert("Failed to Get CostId.");
        }
    });

    //Get Data Car
    $.ajax({
        type: 'GET',
        url: '/Cars/GetCarToList/',
        success: function (data) {
            const $dropdown = $("#add-carId");
            $dropdown.empty().append(`<option>--Select Car--</option>`);
            $.each(data, function (i, car) {
                $dropdown.append(`<option value="${car.chassisNumber}">${car.carName}</option>`);
            });
        },
        error: function () {
            alert("Failed to load suppliers.");
        }
    });

    $("#addCostModal").modal("show");
});

$("#AddCost").off().on("click", function () {

    const costId = $("#add-costId").val()
    const costName = $("#add-costName").val();
    const costValue = $("#add-costValue").val();
    const carId = $("#add-carId").val();


    $("#AddMessage").hide();

    $.ajax({
        type: 'POST',
        url: '/Costs/AddCost/',
        data: {
            CostId: costId,
            CostName: costName,
            CostValue: costValue,
            CarId: carId
        },
        success: function (response) {
            if (response.success) {
                $("#AddMessage").text("Cost Added successfully!").show();
                setTimeout(() => {
                    $("#AddCostModal").modal("hide");
                    //  location.reload();
                }, 1000);
            } else {
                $("#AddMessage").text("Add failed!").show();
            }
        },
        error: function () {
            $("#AddMessage").text("Server error during Adding!").show();
        }


    });


});

$("#editCost").off().on("click", function () {

    const CostId = $("#CostId").val()
    const CostName = $("#CostName").val();
    const CostValue = $("#CostValue").val();
    const ChassisNumber = $("#chassisNumber").val();


    $("#EditMessage").hide();

    $.ajax({
        type: 'POST',
        url: '/Costs/UpdateCost',
        data: {
            costId: CostId,
            costName: CostName,
            costValue: CostValue,
            chassisNumber: ChassisNumber,
        },
        success: function (response) {
            if (response.success) {
                $("#EditMessage").text("Cost Updated successfully!").show();
                setTimeout(() => {
                    $("#editCostModal").modal("hide");
                    location.reload();
                }, 1000);
            } else {
                $("#EditMessage").text("Update failed!").show();
            }
        },
        error: function () {
            $("#EditMessage").text("Server error during update!").show();
        }


    });


});

function DeleteCost(cost_Id, CostName) {

    $("#infoCost").text(cost_Id + " " + CostName); 
  
    $("#deleteMessage").hide();

    $("#deleteCostModal").modal("show");

    $("#confirmDeleteCostBtn").off("click").on("click", function () {

        $.ajax({
            type: 'POST',
            url: '/Costs/DeleteCost',
            data: { costId: cost_Id },
            success: function () {
                $("#deleteMessage").text("Cost deleted successfully!").show();

                setTimeout(() => {
                    $("#deleteCostModal").modal("hide");
                    location.reload();
                }, 1000);

            },
            error: function () {
                $("#deleteMessage").text("Cost deleted Failed!");
            }

        });


    });

}














