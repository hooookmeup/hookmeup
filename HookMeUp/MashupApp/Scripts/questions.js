


function Question(id, name, optionlst) {
    this.name = name;
    this.optionlst = optionlst;
}



function QuestionController($scope) {
    $scope.questionList = [new Question("1", "Which state's capital is Albany", ["Connecticut", "New York", "New Jersy", "Delhi"]),
    new Question("2", "Which state's capital is Nevada", ["Connecticut", "New York", "New Jersy", "LasVegas"])];


}