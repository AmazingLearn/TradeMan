/**
 * Function to validate user numerical input.
 * @param numString
 */
export function validateNumericalInput(numString: string): boolean{

    console.log("The thing is: " + numString);

    const valid = /^\d+$/.test(numString) || /^\d+\.?\d*$/.test(numString) || numString === "";

    if (!valid)
    {
        alert("Invalid numerical value");
    }

    return valid;
}