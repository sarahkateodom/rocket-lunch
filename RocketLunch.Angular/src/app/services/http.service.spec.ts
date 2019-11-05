import { HttpService } from "./http.service";
import { Response, ResponseOptions } from "@angular/http";
import { BehaviorSubject, Subject } from "rxjs";

describe("HttpService", () => {
    let routerSpy: any;
	let httpSpy: any;
	let locationSpy: any;
    let responseSubject: Subject<Response>;
    let subject: HttpService;

    beforeEach(() => {
        httpSpy = jasmine.createSpyObj("http", ["request", "get", "post", "put", "delete"]);
		routerSpy = jasmine.createSpyObj("router", ["navigate"]);
		locationSpy = jasmine.createSpyObj('location', ['path']);

        responseSubject = new BehaviorSubject<Response>(new Response(new ResponseOptions()));
        httpSpy.request.and.returnValue(responseSubject.asObservable());
        httpSpy.get.and.returnValue(responseSubject.asObservable());
        httpSpy.post.and.returnValue(responseSubject.asObservable());
        httpSpy.put.and.returnValue(responseSubject.asObservable());
		httpSpy.delete.and.returnValue(responseSubject.asObservable());
		
		locationSpy.path.and.returnValue('/jwt');
        
        subject = new HttpService(httpSpy, routerSpy);
    });

    describe("get", () => {
        it("should make request", () => {
            subject.get("");          
            expect(httpSpy.get).toHaveBeenCalled();
        });
    });

    describe("post", () => {
        it("should make request", () => {  
            subject.post("", null);             
            expect(httpSpy.post).toHaveBeenCalled();
        });
    });

    describe("put", () => {
        it("should make request", () => { 
            subject.put("", null);         
            expect(httpSpy.put).toHaveBeenCalled();
        });
    });

    describe("delete", () => {
        it("should make request", () => {  
            subject.delete("");        
            expect(httpSpy.delete).toHaveBeenCalled();
        });
    });
});