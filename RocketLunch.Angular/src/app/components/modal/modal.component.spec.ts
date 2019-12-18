import { async, ComponentFixture, TestBed, fakeAsync, tick  } from '@angular/core/testing';

import { ModalComponent } from './modal.component';

describe('ModalComponent', () => {
	let component: ModalComponent;
	let fixture: ComponentFixture<ModalComponent>;

	beforeEach(() => {
		TestBed.configureTestingModule({
			declarations: [ModalComponent]
		})
		.overrideTemplate(
			ModalComponent,
			"<html>HTML for the component requires all dependent components to be loaded. Differ this to Feature test.</html>").compileComponents();

		fixture = TestBed.createComponent(ModalComponent);
		component = fixture.componentInstance;
		fixture.detectChanges();
	});

	it('should create ModalComponent', () => {
		expect(component).toBeTruthy();
	});

	describe('show', () => {
		it('should set visible to true', () => {
			expect(component.visible).toBeFalsy();
			component.show();
			expect(component.visible).toBeTruthy();
		});
		
		it('should set visibleAnimate to true', fakeAsync(() => {
			expect(component.visibleAnimate).toBeFalsy();
			component.show();
			tick(101)
			expect(component.visibleAnimate).toBeTruthy();
		}));
	});

	describe('hide', () => {
		it('should set visibleAnimate to false', () => {
			component.visibleAnimate = true;
			component.hide();
			expect(component.visibleAnimate).toBeFalsy();
		});
		
		it('should set visible to false', fakeAsync(() => {
			component.visible = true;
			component.hide();
			tick(301)
			expect(component.visible).toBeFalsy();
		}));
	});

	describe('onContainerClicked', () => {
		it('should set visibleAnimate to false, when event contains modal', () => {
			// Arrange
			component.visibleAnimate = true;
			let event = createMouseEvent(true, 'modal');

			// Act
			component.onContainerClicked(event);

			// Assert
			expect(component.visibleAnimate).toBeFalsy();
		});

		it('should not set visibleAnimate to false, when event does not modal', () => {
			// Arrange
			component.visibleAnimate = true;
			let event = createMouseEvent(false);

			// Act
			component.onContainerClicked(event);

			// Assert
			expect(component.visibleAnimate).toBeTruthy();
		});
	});

	function createMouseEvent(hasClass, clazz = ''): MouseEvent {
		const event = { target: { classList: { contains: (arg) => false } } }
		if (hasClass) {
			event.target.classList.contains = (cls) => {
				return cls === clazz;
			}
		}
		return <any>event;
	}
});
