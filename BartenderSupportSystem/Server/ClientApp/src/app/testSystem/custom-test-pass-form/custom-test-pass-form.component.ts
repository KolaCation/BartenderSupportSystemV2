import { Component, OnInit } from '@angular/core';
import {
  AbstractControl,
  FormArray,
  FormBuilder,
  FormGroup,
} from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { AuthorizeService } from 'src/api-authorization/authorize.service';
import { CustomValidators } from 'src/app/shared/CustomValidators';
import { ErrorHandlerService } from 'src/app/shared/ErrorHandlerService';
import Swal from 'sweetalert2';
import { ICustomTestResult } from '../custom-test-result/ICustomTestResult';
import { ICustomAnswer } from '../custom-test/custom-questions/custom-answers/ICustomAnswer';
import { ICustomQuestion } from '../custom-test/custom-questions/ICustomQuestion';
import { CustomTestService } from '../custom-test/custom-test.service';
import { ICustomTest } from '../custom-test/ICustomTest';
import { TestResultHelpers } from '../TestResultHelpers';
import { CustomTestResultService } from '../custom-test-result/custom-test-result.service';

@Component({
  selector: 'app-custom-test-pass-form',
  templateUrl: './custom-test-pass-form.component.html',
  styleUrls: ['./custom-test-pass-form.component.css'],
})
export class CustomTestPassFormComponent implements OnInit {
  origin: ICustomTest;
  testToPass: ICustomTest;
  testToPassForm: FormGroup;
  username: string;

  constructor(
    private _formBuilder: FormBuilder,
    private _customTestService: CustomTestService,
    private _activatedRoute: ActivatedRoute,
    private _router: Router,
    private _errService: ErrorHandlerService,
    private _authorizeService: AuthorizeService,
    private _customTestResultService: CustomTestResultService
  ) {}

  ngOnInit(): void {
    this.testToPassForm = this._formBuilder.group({
      questions: this._formBuilder.array([]),
    });
    this.origin = {
      id: 0,
      name: null,
      topic: null,
      description: null,
      questions: null,
      authorUsername: null,
    };
    this.testToPass = {
      id: 0,
      name: null,
      topic: null,
      description: null,
      questions: null,
      authorUsername: null,
    };
    this._activatedRoute.paramMap.subscribe((params) => {
      const id = +params.get('id');
      this._customTestService.getCustomTest(id).subscribe(
        (data: ICustomTest) => {
          this._authorizeService.getUserName().subscribe(
            (name: string) => {
              this.username = name;
              this.origin = data;
              this.testToPass = TestResultHelpers.prepareTestForPassing(
                this.origin
              );
              this.testToPassForm.setControl(
                'questions',
                this.fromModelToQuestionFormArray(this.testToPass.questions)
              );
            },
            (err) => {
              console.log(err);
              Swal.fire({
                position: 'center',
                icon: 'error',
                title: 'Oops...',
                text: 'Something went wrong!',
              });
            }
          );
        },
        (err) => {
          console.log(err);
          Swal.fire({
            position: 'center',
            icon: 'error',
            title: 'Oops...',
            text: 'Something went wrong!',
          });
        }
      );
    });
  }

  onSubmit(): void {
    this.mapFormValuesToModel();
    console.log(this.testToPass);
    console.log(this.origin);
    const result: ICustomTestResult = TestResultHelpers.createResultFromTest(
      this.testToPass
    );
    TestResultHelpers.setMarkAndUsername(result, this.origin, this.username);
    this._customTestResultService.createCustomTestResult(result).subscribe(
      () => {
        this._router.navigate(['/tests/results']);
      },
      (err) => {
        console.log(err);
        Swal.fire({
          position: 'center',
          icon: 'error',
          title: 'Oops...',
          text: 'Something went wrong!',
        });
      }
    );
  }

  //#region Mappers

  mapFormValuesToModel(): void {
    this.testToPass.questions = this.fromQuestionFormArrayToModel(
      this.testToPassForm.get('questions')
    );
  }

  fromQuestionFormArrayToModel(control: AbstractControl): ICustomQuestion[] {
    if (control && control instanceof FormArray) {
      const values: any[] = (<FormArray>control).value;
      const questionsArr: ICustomQuestion[] = [];
      values.forEach((value) => {
        const question: ICustomQuestion = {
          id: +value.questionId,
          testId: +this.testToPass.id,
          statement: value.questionStatement,
          answers: this.fromAnswerFormArrayToModel(
            value.questionId,
            value.answers
          ),
        };
        questionsArr.push(question);
      });
      return questionsArr;
    }
  }
  fromModelToQuestionFormArray(questions: ICustomQuestion[]): FormArray {
    const formArr: FormArray = new FormArray([]);
    questions.forEach((question) => {
      formArr.push(
        this._formBuilder.group({
          questionId: question.id,
          questionStatement: question.statement,
          answers: this.fromModelToAnswerFormArray(question.answers),
        })
      );
    });
    return formArr;
  }
  fromModelToAnswerFormArray(answers: ICustomAnswer[]): FormArray {
    const formArr: FormArray = new FormArray(
      [],
      [CustomValidators.atLeastOneCorrectAnswerExists()]
    );
    answers.forEach((answer) => {
      formArr.push(
        this._formBuilder.group({
          answerId: answer.id,
          questionId: answer.questionId,
          answerIsCorrect: answer.isCorrect,
          answerStatement: answer.statement,
        })
      );
    });
    return formArr;
  }
  fromAnswerFormArrayToModel(
    questionId: number,
    answers: any[]
  ): ICustomAnswer[] {
    const answersArr: ICustomAnswer[] = [];
    answers.forEach((value) => {
      const answer: ICustomAnswer = {
        id: +value.answerId,
        questionId: +questionId,
        statement: value.answerStatement,
        isCorrect: value.answerIsCorrect,
      };
      answersArr.push(answer);
    });
    return answersArr;
  }
  //#endregion
}
