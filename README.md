# DiceBot
需要Another-Mirai-Native 1.6.4以上
## 使用流程
> r
- r => [1, 6] => 3
- r3 => [1, 3] => 2
- r2.3 => [1, 2.3] => 1.4852

> 1d6
- 1d6 => 1 * [1, 6] => 4
- 2d6 => 2 * [1, 6] => 1,5
- 3d6 => 3 * [1, 6] => 2,6,5\nSum: 13.0000 Avg: 4.3333 Min: 2.0000 Max: 6.0000
- 2d2.5 => 2* [1, 2.5] => 1.0870,1.8756

> [@Bot] A or B
- [@Bot] A or B => A
- [@Bot] A or B or C => C
- [@Bot] A 还是 B or C => C
- [@Bot] A 还是 B Or C => B
- 重复同一条件 至多添加3个叹号 条件绑定QQ 保存3小时后销毁 => B！=> B！！=> B！！！
