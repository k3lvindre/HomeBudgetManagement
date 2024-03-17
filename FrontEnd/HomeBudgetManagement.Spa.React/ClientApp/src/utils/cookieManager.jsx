//use anonymous function instead of arrow function =>
//this is how to use non anonymous function:
//function getCookie(name) { }
export const getCookie = function(name) 
{
    const cookieName = encodeURIComponent(name) + '=';
    const cookieArray = document.cookie.split(';');
    for (let i = 0; i < cookieArray.length; i++) {
        let cookie = cookieArray[i];
        while (cookie.charAt(0) === ' ') {
            cookie = cookie.substring(1);
        }
        if (cookie.indexOf(cookieName) === 0) {
            let origCookie = cookie.substring(cookieName.length, cookie.length);
            let result = decodeURIComponent(origCookie)
            return result;
        }
    }
    return null;
}