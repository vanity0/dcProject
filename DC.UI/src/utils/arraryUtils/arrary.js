

/**
 * let arr =  [1,2,2,4,null,null,'3','abc',3,5,4,1,2,2,4,null,null,'3','abc',3,5,4] 
 * // arr: [1, 2, 4, null, "3", "abc", 3, 5]    
 */

/**
 * 利用对象的 key唯一
 * 对象的key不可重复，否则后者将覆盖前者。利用该特性，实现数组去重，遍历数组，将数组的每一项做为对象的key值
 * 存在一定的性能问题
 * @param {要去重的数组} arr 
 */
export const removeDuplicate1 = (arr) => {
    let obj = {};
    for (let i = 0; i < arr.length; i++) {
        let item = arr[i]
        if (obj[item] !== undefined) {
            arr.splice(i, 1);
            i--; // 解决删除元素后，数组塌陷问题
            continue;
        }
        obj[item] = item
    }
}


/**
 * 交换元素位置从而替换调 splice方法
 * 基于splice实现删除性能不太好，当前项被删除后，随后每一项的索引都要向前移动一位，数据量较庞大时，一定会影响性能。
 * 交换元素的位置，效率会更高一点，若当前元素重复，则与数组最后一位元素交换位置，i--再次进行判断即可，同时length--,操作数组的长度实现删除数组的最后一个元素，这样便不会影响到数组中其他元素
 * @param {要去重的数组} arr 
 */
export const removeDuplicate2 = (arr) => {
    let obj = {};
    for (let i = 0; i < arr.length; i++) {
        let item = arr[i]
        if (obj[item] !== undefined) {
            arr[i] = arr[arr.length - 1]
            arr.length--;
            i--;
            continue;
        }
        obj[item] = item
    }
}


/**
 * 返回 item 第一次出现的位置等于当前的index的元素
 * Array.filter + Array.indexOf
 * filter() 方法：创建一个新数组，新数组中的元素是指定数组中符合某种条件的所有元素。如果没有符合条件的元素则返回空数组
 * filter() 不会对空数组进行检测。
 * filter() 不会改变原始数组。
 * @param {要去重的数组} arr 
 * @returns 
 */
export const removeDuplicate3 = (arr) => {
    return arr.filter((item, index) => arr.indexOf(item) === index)
}

/**
 * 利用对象的键名不可重复的特点
 * hasOwnProperty() 方法：返回一个布尔值，表示对象自身属性中是否具有指定的属性
 * @param {要去重的数组} arr 
 * @returns 
 */
export const removeDuplicate4 = (arr) => {
    let obj = {}
    return arr.filter(item => obj.hasOwnProperty(typeof item + item) ? false : (obj[typeof item + item] = true))
}

/**
 *  Array.reduce + Array.includes
 * reduce() 方法：接收一个函数作为累加器，数组中的每个值从左到右开始计算，最终计算为一个值
 * 语法：arr.reduce(function(total, currValue, currIndex, arr), initValue)
 * reduce() 对于空数组是不会执行回调函数的。
 * total：必需。初始值, 或者计算结束后的返回值
 * currValue：必需。当前元素
 * currIndex：可选。当前元素的索引
 * arr ：可选。当前数组对象。
 * initValue：可选。累加器初始值
 * 一个空数组调用reduce()方法且沒有提供初始值，会报错。
 * 一个空数组调用reduce()方法且提供了初始值，将直接返回该初始值，不會调用 callback 函数。
 * 非空数组调用reduce()提供初始值，则total将会等于初始值，且 currValue从第一个元素开始；若沒有提供初始值，则 total 会等于的第一个元素值，且 currValue将会从第二个元素开始。
 * @param {要去重的数组} arr 
 */
export const removeDuplicate5 = (arr) => {
    return arr.reduce((accu, cur) => {
        return accu.includes(cur) ? accu : accu.concat(cur);  // 1. 拼接方法
        // return accu.includes(cur) ? accu : [...accu, cur]; // 2. 扩展运算
    }, [])
}

/**
 * Array.indexOf
 * indexOf() 方法：返回数组中某个指定的元素位置。该方法遍历数组，查找有无对应元素并返回元素第一次出现的索引，未找到指定元素则返回 -
 * @param {要去重的数组} arr 
 */
export const removeDuplicate6 = (arr) => {
    let newArr = []
    for (var i = 0; i < arr.length; i++) {
        if (newArr.indexOf(arr[i]) === -1) newArr.push(arr[i])
    }
    //等同于 forEach 写法
    arr.forEach(item => newArr.indexOf(item) === -1 ? newArr.push(item) : '')
    return newArr
}

/**
 * Array.includes
 * includes() 方法：用来判断一个数组是否包含一个指定的值，如果是返回 true，否则false。
 * @param {要去重的数组} arr 
 */
export const removeDuplicate7 = (arr) => {
    let newArr = []
    for (var i = 0; i < arr.length; i++) {
        if (!newArr.includes(arr[i])) newArr.push(arr[i])
    }
    //等同于 forEach 写法
    arr.forEach(item => !newArr.includes(item) ? newArr.push(item) : '')

    return newArr
}

/**
 * new Set + 扩展运算符 || Array.from
 * ES6 提供了新的数据结构 Set。类似于数组，但是成员的值都是唯一的，没有重复的值。
 * Set本身是一个构造函数，可以接受一个具有 iterable 接口数据结构作为参数（如数组，字符串），用来初始化。
 * @param {要去重的数组} arr 
 */
export const removeDuplicate8 = (arr) => {
    let newArr = [...new Set(arr)];      // [1, 2, 4, null, "3", "abc", 3, 5]
    // let newArr = Array.from(new Set(arr));      // [1, 2, 4, null, "3", "abc", 3, 5]
    // let newStr = [...new Set('ababbc')].join('')  //  'abc'
    return newArr
}

/**
 * new Map
 * ES6 提供了新的数据结构 Map。类似于对象，也是键值对的集合，但是“键”的范围不限于字符串，各种类型的值（包括对象）都可以当作键。
 * set方法设置键名key对应的键值为value，然后返回整个 Map 结构。如果key已经有值，则键值会被更新，否则就新生成该键。
 * get方法读取key对应的键值，如果找不到key，返回undefined。
 * has方法返回一个布尔值，表示某个键是否在当前 Map 对象之中。
 * @param {要去重的数组} arr 
 */
export const removeDuplicate9 = (arr) => {
    let map = new Map();
    let newArry = [];
    for (let i = 0; i < arr.length; i++) {
        if (!map.has(arr[i])) {
            map.set(arr[i], true);
            newArry.push(arr[i]);
        }
    }
    return newArry
}