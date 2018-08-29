//Removing and renaming class names
$('.Profile').addClass('profile').removeClass('Profile');
$('.Buildings').addClass('buildings').removeClass('Buildings');
$('.Create').addClass('create').removeClass('Create');
$('.Configure').addClass('configure').removeClass('Configure');
$('.Edit').addClass('edit').removeClass('Edit');

//Inserting FontAwesome Icons
$('span.account-link-name').each(function () {
    if ($(this).text() == 'Profile') {
        $("<i class='profile-icon fa fa-user'></i>").insertBefore(this);
    }
    if ($(this).text() == 'Buildings') {
        $("<i class='building-icon fa fa-building'></i>").insertBefore(this);
    }
});
$('.building-link').each(function () {
    $("<i class='create-icon fa fa-plus'></i>").insertBefore('.create');
});
$('span.buildings-link-name').each(function () {
    if ($(this).text() == 'Configure') {
        $("<i class='configure-icon fa fa-cogs'></i>").insertBefore(this);
    }
    if ($(this).text() == 'Edit') {
        $("<i class='edit-icon fa fa-pencil-square-o'></i>").insertBefore(this);
    }
});