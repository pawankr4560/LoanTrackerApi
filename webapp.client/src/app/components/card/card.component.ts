import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { CardService } from 'src/app/services/card.service';

@Component({
  selector: 'app-card',
  templateUrl: './card.component.html',
  styleUrls: ['./card.component.css']
})
export class CardComponent implements OnInit {
  cards : any;
  constructor(
    private cardService : CardService
  ) {
  }
  
  ngOnInit(): void {
    this.cardService.getCards().subscribe({
      next :async (response)=>{
         this.cards = response;
         console.log(this.cards);
      },
      error : (err)=>{
      }
    })
  }

  setDefaultCard(cardId : string)
  {
   this.cardService.defaultCard(cardId).subscribe({
    next : (response)=>{
      this.ngOnInit();
    },
    error :(err)=>{
    }
   })
  }

  defaultCard()
  {
    this.cardService.getdefaultCard().subscribe({
      next : (response)=>{
      },
      error :(err)=>{
      }
    })
  }
}
