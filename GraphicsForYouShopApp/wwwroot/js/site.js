

const hamburger = document.querySelector(".navbar-toggler");
const navMenu = document.querySelector("ul");

hamburger.addEventListener("click", function(){
    hamburger.classList.toggle("active");
    navMenu.classList.toggle("active");
});

document.querySelectorAll(".navbar-link").forEach(n => n.addEventListener("click", () => {
    hamburger.classList.remove("active");
    navMenu.classList.remove("active");
}));

$(function () {
    $(window).on('scroll', function () {
        if ($(window).scrollTop() > 10) {
            $('nav').addClass('active-menu');
        } else {
            $('nav').removeClass('active-menu');
        }
    });
});

function addGraphic() {
    window.location.href = "Create";
}


function addCollection(){
    window.location.href = "CreateCollection";
}

$(document).ready(function () {
    updateCartProducts();
    UnreadMessages();
});
function updateCartProducts() {
    var cartProducts;
    var exisitingCookieData = $.cookie('GraphicsInCart');
    if (exisitingCookieData != undefined && exisitingCookieData != "" && exisitingCookieData != null) {
        cartProducts = exisitingCookieData.split('-');
    } else {
        cartProducts = [];
    }

    $("#cartProductsCount").html(cartProducts.length)
}



function AddGraphicsToPromotion(){

    let graphicsCheckboxes = document.getElementsByName("graphicWithPromotion");
    let graphics = [];

    for(let i = 0; i < graphicsCheckboxes.length; i++)
    {  
        if(graphicsCheckboxes[i].checked)  {
        graphics.push(graphicsCheckboxes[i].value)
    }
    }

    document.getElementById("graphicsChosen").value = JSON.stringify(graphics);
    
}

function addPromotion() {
    window.location.href = "CreatePromotion";
}



    var input = document.getElementById("key");

    input.addEventListener("keypress", function(event) {
      if (event.key === "Enter") {
        let key = document.getElementById('key').value;
        if(key.length < 3){
            return false;
        }
        localStorage.clear();
            localStorage.setItem('key', key);
            location.href = '/Graphic/Search?key='+key;
      }
    });



   
function ChooseCategory(id) {
    var categories = [];
        categories.push(id);
        localStorage.clear();
        localStorage.setItem('categories', JSON.stringify(categories));
        location.href = '/Graphic/Search?categories='+JSON.stringify(categories);
    }

function ViewMessage(ids) {
    var usersss = document.getElementsByClassName("users-to-message");
    for (var i = 0; i < usersss.length; i++) {
        usersss[i].style.backgroundColor = "white";
    }
    var userrr = document.getElementById('user+' + ids);
    userrr.style.backgroundColor = "#e8e8e8";

    $("#project-contener-right").load("/Account/Communicator",
        { id: ids });
    $('#project-contener-right').scrollTop($('#project-contener-right')[0].scrollHeight);

    document.getElementById("choose-conversation").style.display = "none";


};


function DeleteFilter() {
    localStorage.clear();
    let categories = [];
    let collections = [];
    location.href = "/Graphic/Search?categories=" + JSON.stringify(categories) + "&collections=" + JSON.stringify(collections);
}

function FilterGraphics() {
    let categoryCheckboxes = document.getElementsByName("graphicCategory");
    let categories = [];
    let collectionCheckboxes = document.getElementsByName("graphicCollection");
    let collections = [];
    for (let i = 0; i < categoryCheckboxes.length; i++) {
        if (categoryCheckboxes[i].checked) {
            categories.push(categoryCheckboxes[i].value)
        }
    }
    for (let i = 0; i < collectionCheckboxes.length; i++) {
        if (collectionCheckboxes[i].checked) {
            collections.push(collectionCheckboxes[i].value)
        }
    }

    localStorage.setItem('key', '');
    localStorage.setItem('categories', JSON.stringify(categories));
    localStorage.setItem('collections', JSON.stringify(collections));
    location.href = "/Graphic/Search?categories=" + JSON.stringify(categories) + "&collections=" + JSON.stringify(collections);
}

function deleteProject(id) {
    var cartProducts;
    var exisitingCookieData = $.cookie('GraphicsInCart');

    cartProducts = exisitingCookieData.split('-');

    cartProducts.forEach(function (element) {
        if (element == id) {
            cartProducts = cartProducts.filter(function (ele) {
                return ele != id;

            });
            console.log(cartProducts);
        }
    });

    var coookies = cartProducts.join("-");

    $.cookie('GraphicsInCart', coookies, { path: '/' });

    location.reload();
    $("#cartProductsCount").html(cartProducts.length);

};



function AddFiles(id){
    location.href = '/Order/FilesSite?id=' + id;
}



function orderDetails(orderId)
{
    window.location.href = '/Order/OrderDetailsDesigner?id=' + orderId;
}


function orderDetail(orderId)
{
    window.location.href = '/Order/OrderDetails?id=' + orderId;
}
