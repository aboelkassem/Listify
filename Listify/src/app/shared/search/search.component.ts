import { OAuthService } from 'angular-oauth2-oidc';
import { SearchService } from './search.service';
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Subscription } from 'rxjs';
import { ISongSearchResult } from 'src/app/interfaces';

@Component({
  selector: 'app-search',
  templateUrl: './search.component.html',
  styleUrls: ['./search.component.css']
})
export class SearchComponent implements OnInit {

  query: string;
  searchResults: Array<ISongSearchResult>;
  // sub: Subscription;


  constructor(
    private searchService: SearchService,
    private oauthService: OAuthService,
    private route: ActivatedRoute) { }

  ngOnInit(): void {
  }

  search(): void {
  //   this.searchService.search(this.query).subscribe((data: any) => {
  //     this.searchResults = data;
  //   },
  //   error => console.log(error));
  }

}
