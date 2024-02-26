/*----------------------------------*\
    #CHALLENGE 2
\*----------------------------------*/

const electionVotes = [
    'Harry', 'Rick', 'Ben', 'Rick', 'Harry', 'Ben', 'Harry', 'Rick',
    'Ben', 'Rick', 'Harry', 'Ben', 'Rick', 'Harry', 'Ben', 'Rick', 'Harry',
    'Ashley', 'Ben', 'Rick', 'Harry', 'Ashley', 'Rick', 'Ben', 'Harry',
    'Michael', 'Rick', 'Ben', 'Harry', 'Michael', 'Rick', 'Ben', 'Harry',
    'Albert', 'Donna', 'Mortimer', 'Michael', 'Faye', 'Harry', 'Mortimer',
    'Rick', 'Ashley', 'Donna', 'Mortimer', 'Faye', 'Harry', 'Mortimer',
    'Rick', 'Ashley', 'Donna', 'Mortimer', 'Faye', 'Rick', 'Mortimer', 'Rick',
    'Albert', 'Donna', 'Mortimer', 'Michael', 'Faye', 'Harry', 'Mortimer',
    'Rick', 'Ashley', 'Donna', 'Mortimer', 'Faye', 'Harry', 'Mortimer',
];

/* Expected Output (something like this):
{
    Harry: 7,
    Rick: 6,
    Ben: 6,
    Ashley: 3,
    Michael: 3,
    Mortimer: 8,
    Donna: 3,
    Faye: 3,
    Albert: 2
}
*/

/* sol#1
--------------------------- 
    const tallyVotes1 = (votes) => {
        let result = {};
        votes.forEach((vote) => {
            if (result[vote]) {
                result[vote] += 1;
            } else {
                result[vote] = 1;
            }
        });
        return result;
    };

*/

/* sol#2
--------------------------- 
    const tallyVotes2 = (votes) => {
        let result = {};
        votes.forEach((vote) => {
            result[vote] = result[vote] ? result[vote] + 1 : 1;
        });
        return result;
    };

*/

/* sol#3
--------------------------- 
    const tallyVotes3 = (votes) => 
        votes.reduce((result, vote) => {
            result[vote] = result[vote] ? result[vote] + 1 : 1;
            return result;
        }, {});

*/

/* sol#4
---------------------------*/
const tallyVotes4 = (votes) =>
votes.reduce((result, vote) => ({
    ...result,
    [vote] : result[vote] ? result[vote] + 1 : 1,
}), {});

/*----------------------------------*\
    Input -> Output Tests
\*----------------------------------*/

console.log(tallyVotes3(electionVotes));