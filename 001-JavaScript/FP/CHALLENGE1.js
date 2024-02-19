/*----------------------------------*\
    CHALLENGE 1
\*----------------------------------*/

const map = (arr, fn) => 
    arr.reduce((acc, item) => [
        ...acc,
        fn(item),
    ], []);  

/* sol#2
---------------------------   
    const map = (arr, fn) =>
    {
        let newArr = [];    
        arr.forEach(element => {
            newArr.push(fn(element));
        });
        return newArr;
    }    
*/

/* sol#1
---------------------------
    const map = (arr, fn) =>
    {
        let newArr = [];
        for (let i = 0; i < arr.length; i++) {
            newArr.push(fn(arr[i]));
        }
        return newArr;
    }    
*/

/*----------------------------------*\
    Input -> Output Tests
\*----------------------------------*/

// Expected Output: [2, 4, 6]
console.log(map([1, 2, 3], x => x * 2));

// Expected Output: [-5, -6, -7, -8, -9, -10]
console.log(map([5, 6, 7, 8, 9, 10], x => -x));

// Expected Output: ['A', 'B', 'C', 'D']
console.log(map(['a', 'b', 'c', 'd'], x => x.toUpperCase()));