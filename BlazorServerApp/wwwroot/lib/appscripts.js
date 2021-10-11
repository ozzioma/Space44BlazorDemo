window.getDimensions = function ()
{
    return {
        width: window.innerWidth,
        height: window.innerHeight
    };
};

window.initAppScripts = function ()
{

   // initAppScripts_Core();


    return {
        successFlag: true,
        errorMessage: ''
    };
};



window.reloadPage = function ()
{
    window.location.reload()
};