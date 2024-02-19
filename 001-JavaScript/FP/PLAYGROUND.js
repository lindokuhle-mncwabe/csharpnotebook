const countUp = (counter, max) => {
    if (counter > max) return;
    console.log(counter);
    countUp(counter + 1, max);
} 

countUp(0, 10)