$(document).ready(function () {
    getSearchResults();

    function getSearchResults() {        
        $("#divSearchResults").addClass("marginLeft");
        $("#divSearchResults").html("Loading documents...");
        $.ajax({
            url: "../Home/GetSearchResult",
            type: "POST",
            dataType: "html",
            contentType: "application/json; charset=UTF-8",
            success: function (result) {
                $("#divSearchResults").html("");
                $("#divSearchResults").append(result);
                populateCategoryButton();                
            },
            error: function () {
                showToastError("Failed to load articles.");
            }
        });
    }

    function populateCategoryButton() {
        $.ajax({
            type: "POST",
            url: "../Home/GetCategories",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (result) {
                var ul = $("#dropdownCategoriesUl");
                $(ul).empty();
                var lastchild;
                jQuery.each(result, function (i, val) {
                    $(ul).append('<li onclick="filterCategories(this.id);" id="' + val.Id + '">' + val.CategoryName + '</li>');
                    var li = $(ul).children()[i];
                    $(li).data(val);
                    lastchild = i;
                });
                $(ul).append('<li onclick="filterCategories(this.id);" id="noFilter"> No filter</li>');
                var li = $(ul).children()[lastchild + 1];
                $(li).data("No filter");
            },
            error: function () {
                showToastError("Failed to load categories.");
            }
        });
    }

    $('#btnGetArticle').click(function () {        
        if ($('#resultCountmaxLimit').attr('maxLimit') == "True") //if theres articles >= 10  in DB, dont allow user to add more
        {
            showToastError("Can't add more than 10 articles in database. Clear database before adding more articles.");
            return;
        }
        //Gets a random wikipedia article
        var url = "https://en.wikipedia.org/w/api.php?action=query&generator=random&grnnamespace=0&prop=extracts&format=json&callback=?";
        var pageId;
        var articleName;
        $.getJSON(url, function (data) {
            var pages = data.query.pages;
            for (var name in pages) {
                articleName = pages[name].title;
                pageId = pages[name].pageid;
            }
            saveArticle(articleName, pageId);
        });
    });

    $('#btnClearDB').click(function () {
        //clears DB
        $.ajax({
            url: "../Home/ClearDB",
            type: "POST",
            dataType: "html",
            contentType: "application/json; charset=UTF-8",
            success: function (result) {
                getSearchResults();
                showToast('Database cleared.')                
            },
            error: function () {
                showToastError("Failed to clear database.");
            }
        });
    });

    function saveArticle(articleName, pageId) {        
        $.ajax({
            url: "../Home/SaveArticle",
            type: "POST",
            dataType: "html",
            contentType: "application/json; charset=UTF-8",
            async: true,
            cache: false,
            data: JSON.stringify({
                articleName: articleName,
                pageId: pageId,
            }),
            success: function (result) {
                getSearchResults();
                showToast('Article: "' + articleName + '" saved')                          
            },
            error: function () {
                showToastError("Failed to save articles.");
            }
        });
    }

    function showToast(message) {
        $().toastmessage('showToast', {
            text: message,
            sticky: false,
            position: "top-right",
            type: "success",
            stayTime: 10000,
        });
    };

    function showToastError(message) {
        $().toastmessage('showToast', {
            text: message,
            sticky: false,
            position: "top-right",
            type: "error",
            stayTime: 10000,
        });
    };

    $('#btnAddCategory').click(function () {
        //adds category to article, and DB if it doesnt exist allready
        var categoryName = $('#inputCategories').val();
        var artId = $('#activeArticle').attr("articleId");
        var artName = $('#activeArticle').attr("articleName");
        $.ajax({
            url: "../Home/AddCategory",
            type: "POST",
            dataType: "html",
            contentType: "application/json; charset=UTF-8",
            async: true,
            cache: false,
            data: JSON.stringify({
                category: categoryName,
                articleId: artId
            }),
            success: function () {
                getSearchResults();
                showToast('Category: "' + categoryName + '" added to ' + artName)                              
            },
            error: function () {
                showToastError("Failed to save articles.");
            }
        });
    })
})

function openModal(url, that, articleId, articleName) {
    //opens wikiarticle modal
    $('#activeArticle').attr("articleId", articleId);
    $('#activeArticle').attr("articleName", articleName);
    $.getJSON(url, function (data) {
        var content;
        var articleName
        var pages = data.query.pages;
        for (var name in pages) {
            content = pages[name].extract;
            articleName = pages[name].title;
        }
        $('#wikiInfo').html(content);
        $('#ModalLable').html(articleName);
        $('#divModal').modal('show');
    });

}

function openCategoryModal(that) {    
    $.ajax({
        url: "../Home/GetCategories",
        type: "POST",
        dataType: "json",
        contentType: "application/json; charset=UTF-8",
        success: function (results) {
            var categories = [];
            jQuery.each(results, function (i, result) {
                categories.push(result.CategoryName);
            })
            $("#inputCategories").val('');
            $('#divModalCategories').modal('show');
            $("#inputCategories").autocomplete({
                source: categories
            });
        },
        error: function () {
            showToastError("Failed to load categories.");
        }
    });
}


function filterCategories(id) {
    //filter searchresults based on user input 
    if (id == "noFilter") {
        $(".searchResult").each(function () {
            $(this).removeClass("hide").addClass("show");
        });
    }
    else {
        var hasCategory = false;
        $(".searchResult").each(function () {
            hasCategory = false;
            $(this).find(".category").each(function () {
                var targetId = $(this)[0].id;
                if (targetId == id)
                    hasCategory = true;
            });
            if (hasCategory == true) {
                var v = $(this);
                $(this).removeClass("hide").addClass("show");
            }
            else if (hasCategory == false) {
                var v = $(this);
                $(this).removeClass("show").addClass("hide");
            }
        });
    }
}
